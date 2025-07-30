using Microsoft.EntityFrameworkCore;
using TesteDengine.Domain.Entities;
namespace TesteDengine;

public class DbExercicio4: DbContext
{
    public DbExercicio4(DbContextOptions<DbExercicio4> options)
        : base(options)
    {

    }

    public DbSet<Fatura> Fatura { get; set; }
    public DbSet<FaturaItem> FaturaItem{ get; set; }
    public DbSet<Cliente> Cliente { get; set; }
        
}