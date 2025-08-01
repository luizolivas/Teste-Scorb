using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services.Interfaces;

namespace TesteDengine.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _clienteService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            return cliente == null ? NotFound() : Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteCreateDTO dto)
        {
            await _clienteService.AddAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ClienteUpdateDTO dto)
        {
            await _clienteService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clienteService.DeleteAsync(id);
            return Ok();
        }
    }
}
