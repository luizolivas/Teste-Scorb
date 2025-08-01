using Microsoft.AspNetCore.Mvc;
using TesteDengine.Application.Services;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services.Interfaces;

namespace TesteDengine.WebMVC.Controllers
{
    public class FaturaItemController : Controller
    {
        private readonly IFaturaItemService _faturaItemService;

        public FaturaItemController(IFaturaItemService faturaItemService)
        {
            _faturaItemService = faturaItemService;
        }

        [HttpGet]
        public IActionResult Create(int faturaId)
        {
            var model = new FaturaItemCreateDTO { FaturaId = faturaId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FaturaItemCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _faturaItemService.AddAsync(dto);

                return RedirectToAction("Details", "Fatura", new { id = dto.FaturaId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }



        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _faturaItemService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            var dto = new FaturaItemUpdateDTO {
                FaturaItemId = item.FaturaItemId,
                Ordem = item.Ordem,
                Valor = item.Valor,
                Descricao = item.Descricao
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FaturaItemUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _faturaItemService.UpdateAsync(dto);
                var item = await _faturaItemService.GetByIdAsync(dto.FaturaItemId);

                return RedirectToAction("Details", "Fatura", new { id = item.FaturaId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }


        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _faturaItemService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            await _faturaItemService.DeleteAsync(id);
            return RedirectToAction("Details", "Fatura", new { id = item.FaturaId });
        }
    }
}
