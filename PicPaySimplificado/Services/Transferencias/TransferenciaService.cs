using Microsoft.EntityFrameworkCore.Storage;
using PicPaySimplificado.Infra.Repository.Carteiras;
using PicPaySimplificado.Infra.Repository.Transferencias;
using PicPaySimplificado.Mappers;
using PicPaySimplificado.Models;
using PicPaySimplificado.Models.DTOs;
using PicPaySimplificado.Models.Enum;
using PicPaySimplificado.Models.Requests;
using PicPaySimplificado.Models.Response;
using PicPaySimplificado.Services.Autorizador;
using PicPaySimplificado.Services.Notificacoes;

namespace PicPaySimplificado.Services.Transferencias;

public class TransferenciaService : ITransferenciaService
{
    private readonly ITransferenciaRepository _transacaoRepository;
    private readonly ICarteiraRepository _carteiraRepository;
    private readonly IAutorizadorService _autorizadorService;
    private readonly INotificacaoService _notificacaoService;

    public TransferenciaService(ITransferenciaRepository transacaoRepository, ICarteiraRepository carteiraRepository,
        IAutorizadorService autorizadorService, INotificacaoService notificacaoService)
    {
        _transacaoRepository = transacaoRepository;
        _carteiraRepository = carteiraRepository;
        _autorizadorService = autorizadorService;
        _notificacaoService = notificacaoService;
    }

    public async Task<Result<TransferenciaDto>> ExecuteAsync(TransferenciaRequest request)
    {
        if (!await _autorizadorService.AuthorizeAsync())
            return Result<TransferenciaDto>.Failure("Não Autorizado");

        var pagador = await _carteiraRepository.GetById(request.SenderId);
        var recebedor = await _carteiraRepository.GetById(request.ReciverId);

        if (pagador is null || recebedor is null)
            return Result<TransferenciaDto>.Failure("Nenhuma Carteira encontrada");

        if (pagador.SaldoConta < request.Valor || pagador.SaldoConta == 0)
            return Result<TransferenciaDto>.Failure("Saldo Insuficiente");

        if (pagador.UserType == UserType.Lojista)
            return Result<TransferenciaDto>.Failure("Lojista não pode efetuar transferencia");

        pagador.DebitarSaldo(request.Valor);
        recebedor.CreditarSaldo(request.Valor);

        var transferencia = new TransferenciaEntity(pagador.Id, recebedor.Id, request.Valor);

        using (var transferenciaScope = await _transacaoRepository.BeginTransactionAsync())

        {
            try
            {
                await ExecutarOperacoesEmParaleloAsync(pagador, recebedor, transferencia);
                await CommitTransacaoAsync(transferenciaScope);
            }
            catch (Exception ex)
            {
                await RollbackTransacaoAsync(transferenciaScope);
                return Result<TransferenciaDto>.Failure("Erro ao realizar a transferência: " + ex.Message);
            }
        }

        await _notificacaoService.SendNotification();
        return Result<TransferenciaDto>.Success(transferencia.ToTransferenciaDto());
    }


    private async Task ExecutarOperacoesEmParaleloAsync(CarteiraEntity pagador, CarteiraEntity recebedor,
        TransferenciaEntity transferencia)
    {
        // Cria as tarefas para atualizar as carteiras e adicionar a transação
        var tarefas = new List<Task>
        {
            _carteiraRepository.UpdateAsync(pagador),
            _carteiraRepository.UpdateAsync(recebedor),
            _transacaoRepository.AddTransaction(transferencia)
        };

        // Aguarda todas as tarefas finalizarem
        await Task.WhenAll(tarefas);
    }

    private async Task CommitTransacaoAsync(IDbContextTransaction transaction)
    {
        await _transacaoRepository.CommitAsync();
        await transaction.CommitAsync();
    }

    private async Task RollbackTransacaoAsync(IDbContextTransaction transaction)
    {
        await transaction.RollbackAsync();
    }
}