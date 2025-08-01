using TesteDengine.Application.Relatorios.Interfaces;
using TesteDengine.Application.Relatorios;
using TesteDengine.Application.Services.Interfaces;
using TesteDengine.Application.Services;
using TesteDengine.Domain.Interfaces;
using TesteDengine.Infrastructure.Repositories;
using TesteDengine;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
