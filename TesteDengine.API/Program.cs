using TesteDengine.Application.Relatorios.Interfaces;
using TesteDengine.Application.Relatorios;
using TesteDengine.Application.Services.Interfaces;
using TesteDengine.Application.Services;
using TesteDengine.Domain.Interfaces;
using TesteDengine.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using TesteDengine;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DbExercicio4>(options =>
    options.UseInMemoryDatabase("DbExercicio4"));


builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IFaturaRepository, FaturaRepository>();
builder.Services.AddScoped<IFaturaItemRepository, FaturaItemRepository>();

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IFaturaService, FaturaService>();
builder.Services.AddScoped<IFaturaItemService, FaturaItemService>();
builder.Services.AddScoped<IFaturaRelatorioService, FaturaRelatorioService>();
builder.Services.AddScoped<ValidadorFaturaItem>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();

builder.Services.AddHttpClient("Api", client =>
{
    // Endereço base da sua API. Altere a porta se necessário.
    client.BaseAddress = new Uri("https://localhost:7258/api/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
