using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Models;

namespace PicPaySimplificado.Infra.Repository.Carteiras;

public class CarteiraRepository : ICarteiraRepository
{
    private readonly ApplicationDbContext _context;

    public CarteiraRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CarteiraEntity carteira)
    {
        await _context.AddAsync(carteira);
    }

    public async Task UpdateAsync(CarteiraEntity carteira)
    {
        _context.Update(carteira);
    }

    public async Task<CarteiraEntity?> GetByCpfCnpj(string cpfCnpj, string email)
    {
        return await _context.Wallets.FirstOrDefaultAsync(wallet =>
            wallet.CPFCNPJ.Equals(cpfCnpj) || wallet.Email.Equals(email));
    }

    public async Task<CarteiraEntity?> GetById(int id)
    {
        return await _context.Wallets.FindAsync(id);
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}