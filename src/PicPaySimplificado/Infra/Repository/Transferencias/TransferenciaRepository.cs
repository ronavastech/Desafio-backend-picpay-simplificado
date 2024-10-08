using Microsoft.EntityFrameworkCore.Storage;
using PicPaySimplificado.Models;

namespace PicPaySimplificado.Infra.Repository.Transferencias;

public class TransferenciaRepository : ITransferenciaRepository
{
    private readonly ApplicationDbContext _context;

    public TransferenciaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddTransaction(TransferenciaEntity entityEntity)
    {
        await _context.Transfers.AddAsync(entityEntity);
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}