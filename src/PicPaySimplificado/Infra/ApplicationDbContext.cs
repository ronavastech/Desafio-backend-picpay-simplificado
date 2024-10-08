using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Models;

namespace PicPaySimplificado.Infra;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<CarteiraEntity?> Wallets { get; set; }
    
    public DbSet<TransferenciaEntity> Transfers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CarteiraEntity>()
            .HasIndex(w => new { w.CPFCNPJ, w.Email })
            .IsUnique();
        
        
        modelBuilder.Entity<CarteiraEntity>()
            .Property(t => t.SaldoConta)
            .HasColumnType("decimal(18, 2)");
        
        modelBuilder.Entity<CarteiraEntity>()
            .Property(w => w.UserType)
            .HasConversion<string>();

        modelBuilder.Entity<TransferenciaEntity>()
            .HasKey(t => t.IdTransferencia);

        modelBuilder.Entity<TransferenciaEntity>()
            .HasOne(t => t.Sender)
            .WithMany()
            .HasForeignKey(t => t.SenderId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Transaction_Sender");
        
        modelBuilder.Entity<TransferenciaEntity>()
            .Property(t => t.Valor)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<TransferenciaEntity>()
            .HasOne(t => t.Reciver)
            .WithMany()
            .HasForeignKey(t => t.ReciverId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Transaction_Reciver");
    }
}