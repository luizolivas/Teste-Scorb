using Microsoft.EntityFrameworkCore;
namespace TesteDengine;

public class DbExercicio4: DbContext
{
    public DbExercicio4(DbContextOptions<DbExercicio4> options)
        : base(options)
    {

    }

    public DbSet<Fatura> Fatura { get; set; }
    public DbSet<FaturaItem> FaturaItem{ get; set; }
        
}