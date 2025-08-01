using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services.Interfaces;

namespace TesteDengine.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaturaItemController : ControllerBase
    {
        private readonly IFaturaItemService _faturaItemService;

        public FaturaItemController(IFaturaItemService faturaItemService)
        {
            _faturaItemService = faturaItemService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _faturaItemService.GetAllAsync());

        [HttpGet("by-fatura/{faturaId}")]
        public async Task<IActionResult> GetByFatura(int faturaId) =>
            Ok(await _faturaItemService.GetAllByFaturaIdAsync(faturaId));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _faturaItemService.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FaturaItemCreateDTO dto)
        {
            await _faturaItemService.AddAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] FaturaItemUpdateDTO dto)
        {
            await _faturaItemService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _faturaItemService.DeleteAsync(id);
            return Ok();
        }
    }
}
