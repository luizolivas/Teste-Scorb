using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TesteDengine.Application.Services.Interfaces;
using TesteDengine.Application.Services;
using TesteDengine.Domain.Interfaces;
using TesteDengine.Infrastructure.Repositories;
using TesteDengine;
using TesteDengine.Application.Relatorios.Interfaces;
using TesteDengine.Application.Relatorios;

public class ConsoleServiceProvider
{
    public readonly ServiceProvider _serviceProvider; 

    public ConsoleServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddDbContext<DbExercicio4>(opt => opt.UseInMemoryDatabase(nameof(DbExercicio4)));

        services.AddScoped<IFaturaRepository, FaturaRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IFaturaItemRepository, FaturaItemRepository>();

        services.AddScoped<IFaturaService, FaturaService>();
        services.AddScoped<IFaturaItemService, FaturaItemService>();
        services.AddScoped<IClienteService, ClienteService>();

        services.AddScoped<IFaturaRelatorioService, FaturaRelatorioService>();

        services.AddScoped<ValidadorFaturaItem>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public ServiceProvider GetServiceProvider()
    {
        return _serviceProvider;
    }

    public T GetService<T>() where T : notnull => _serviceProvider.GetRequiredService<T>();

    static ConsoleServiceProvider() => Instance = new ConsoleServiceProvider();
    public static ConsoleServiceProvider Instance { get; }
}