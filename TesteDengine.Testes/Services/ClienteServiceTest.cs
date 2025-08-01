using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services;
using TesteDengine.Domain.Entities;
using TesteDengine.Domain.Interfaces;

namespace TesteDengine.Testes.Services
{
    public class ClienteServiceTest
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly ClienteService _clienteService;

        public ClienteServiceTest()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _clienteService = new ClienteService(_clienteRepositoryMock.Object);
        }

        [Fact]
        public async Task AddAsync_DeveAdicionarCliente()
        {
            var dto = new ClienteCreateDTO { Nome = "Luiz" };

            await _clienteService.AddAsync(dto);

            _clienteRepositoryMock.Verify(r => r.AddAsync(It.Is<Cliente>(c => c.Nome == "Luiz")), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarTodosClientes()
        {
            var clientes = new List<Cliente>
            {
                new Cliente { ClienteId = 1, Nome = "Maria" },
                new Cliente { ClienteId = 2, Nome = "Pedro" }
            };

            _clienteRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(clientes);

            var resultado = await _clienteService.GetAllAsync();

            Assert.Collection(resultado,
                c => Assert.Equal("Maria", c.Nome),
                c => Assert.Equal("Pedro", c.Nome));
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarCliente()
        {
            var cliente = new Cliente { ClienteId = 1, Nome = "Ana" };
            _clienteRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cliente);

            var resultado = await _clienteService.GetByIdAsync(1);

            Assert.NotNull(resultado);
            Assert.Equal("Ana", resultado?.Nome);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNull_SeNaoEncontrado()
        {
            _clienteRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Cliente?)null);

            var resultado = await _clienteService.GetByIdAsync(99);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task UpdateAsync_DeveAtualizarCliente()
        {
            var cliente = new Cliente { ClienteId = 1, Nome = "Antigo" };
            _clienteRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cliente);

            var dto = new ClienteUpdateDTO { ClienteId = 1, Nome = "Novo" };

            await _clienteService.UpdateAsync(dto);

            Assert.Equal("Novo", cliente.Nome);
            _clienteRepositoryMock.Verify(r => r.UpdateAsync(cliente), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeveExcluirCliente()
        {
            var cliente = new Cliente { ClienteId = 1, Nome = "Luiz" };
            _clienteRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cliente);

            await _clienteService.DeleteAsync(1);

            _clienteRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeveLancarExcecao_SeNaoEncontrarCliente()
        {
            _clienteRepositoryMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Cliente?)null);

            var ex = await Assert.ThrowsAsync<Exception>(() => _clienteService.DeleteAsync(2));

            Assert.Contains("não encontrado", ex.Message);
        }
    }
}
