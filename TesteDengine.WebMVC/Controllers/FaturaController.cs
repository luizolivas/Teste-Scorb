using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services.Interfaces;

namespace TesteDengine.WebMVC.Controllers
{
    public class FaturaController : Controller
    {
        private readonly IFaturaService _faturaService;
        private readonly IClienteService _clienteService;

        public FaturaController(IFaturaService faturaService, IClienteService clienteService)
        {
            _faturaService = faturaService;
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index()
        {
            var faturas = await _faturaService.GetAllWithDetailsAsync();
            return View(faturas);
        }

        public async Task<IActionResult> Details(int id)
        {
            var fatura = await _faturaService.GetByIdWithDetailsAsync(id);
            if (fatura == null) return NotFound();
            return View(fatura);
        }

        public async Task<IActionResult> Create()
        {
            var clientes = await _clienteService.GetAllAsync();
            ViewData["Clientes"] = new SelectList(clientes, "ClienteId", "Nome");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FaturaCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _faturaService.AddAsync(dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var fatura = await _faturaService.GetByIdWithDetailsAsync(id);
            if (fatura == null) return NotFound();

            var updateDto = new FaturaUpdateDTO {
                FaturaId = fatura.FaturaId,
                ClienteId = fatura.Cliente.ClienteId,
                Data = fatura.Data
            };

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FaturaUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _faturaService.UpdateAsync(dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var fatura = await _faturaService.GetByIdAsync(id);
            if (fatura == null) return NotFound();
            return View(fatura);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _faturaService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
