using PicPaySimplificado.Infra.Repository.Carteiras;
using PicPaySimplificado.Models;
using PicPaySimplificado.Models.Requests;
using PicPaySimplificado.Models.Response;

namespace PicPaySimplificado.Services.Carteiras;

public class CarteiraService : ICarteiraService
{
    private readonly ICarteiraRepository _repository;

    public CarteiraService(ICarteiraRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> ExecuteAsync(CarteiraRequest request)
    {
        var walletsExists = await _repository.GetByCpfCnpj(request.CPFCNPJ, request.Email);

        if (walletsExists is not null)
            return Result<bool>.Failure("Carteira já existe");

        var wallet = new CarteiraEntity(
            request.NomeCompleto,
            request.CPFCNPJ,
            request.Email,
            request.Senha,
            request.UserType,
            request.SaldoConta);

        await _repository.AddAsync(wallet);
        await _repository.CommitAsync();

        return Result<bool>.Success(true);
    }
}