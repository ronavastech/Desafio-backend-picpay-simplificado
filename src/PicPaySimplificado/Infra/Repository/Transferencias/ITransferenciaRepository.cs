using Microsoft.EntityFrameworkCore.Storage;
using PicPaySimplificado.Models;

namespace PicPaySimplificado.Infra.Repository.Transferencias;

public interface ITransferenciaRepository
{
    Task AddTransaction(TransferenciaEntity entityEntity);
    
    Task CommitAsync();
    
    Task<IDbContextTransaction> BeginTransactionAsync();
}