using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services.Interfaces;

namespace TesteDengine.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaturaController : ControllerBase
    {
        private readonly IFaturaService _faturaService;

        public FaturaController(IFaturaService faturaService)
        {
            _faturaService = faturaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _faturaService.GetAllAsync());

        [HttpGet("with-details")]
        public async Task<IActionResult> GetWithDetails() => Ok(await _faturaService.GetAllWithDetailsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await _faturaService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FaturaCreateDTO dto)
        {
            await _faturaService.AddAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] FaturaUpdateDTO dto)
        {
            await _faturaService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _faturaService.DeleteAsync(id);
            return Ok();
        }
    }

}
