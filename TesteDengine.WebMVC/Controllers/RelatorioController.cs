using Microsoft.AspNetCore.Mvc;
using TesteDengine.Application.Relatorios.DTOs;
using TesteDengine.Application.Relatorios.Interfaces;
using TesteDengine.Application.Services.DTOs;

namespace TesteDengine.WebMVC.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly IFaturaRelatorioService _relatorioService;

        public RelatorioController(IFaturaRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> TopFaturas()
        {
            try
            {
                var result = await _relatorioService.ObterTop10FaturasAsync();
                return View(result);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View(new List<FaturaDTO>());
            }
        }

        public async Task<IActionResult> TopItens()
        {
            try
            {
                var result = await _relatorioService.ObterTop10ItensAsync();
                return View(result);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View(new List<FaturaItemDTO>());
            }
        }

        public async Task<IActionResult> TotalPorCliente()
        {
            try
            {
                var result = await _relatorioService.ObterTotalPorClienteAsync();
                return View(result);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View(new List<TotalPorClienteDTO>());
            }
        }

        public async Task<IActionResult> TotalPorMesAno()
        {
            try
            {
                var result = await _relatorioService.ObterTotalPorMesAnoAsync();
                return View(result);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View(new List<TotalPorMesAnoDTO>());
            }
        }
    }
}
