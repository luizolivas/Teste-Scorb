using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TesteDengine;

public class ConsoleServiceProvider
{
    private readonly ServiceProvider _serviceProvider;

    public ConsoleServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddDbContext<DbExercicio4>(opt => opt.UseInMemoryDatabase(nameof(DbExercicio4)));
        _serviceProvider = services.BuildServiceProvider();
    }

    public DbExercicio4 CreateDbExercicio4() => _serviceProvider.GetService<DbExercicio4>()!;

    static ConsoleServiceProvider() => Instance = new ConsoleServiceProvider();
    public static ConsoleServiceProvider Instance { get; }
}