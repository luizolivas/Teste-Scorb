using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TesteDengine.Application.Services;
using TesteDengine.Domain.Interfaces;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Domain.Entities;
using TesteDengine;


namespace TesteDengine.Testes.Services
{
    public class FaturaServiceTest
    {
        private readonly Mock<IFaturaRepository> _mockFaturaRepo = new();
        private readonly Mock<IClienteRepository> _mockClienteRepo = new();

        private FaturaService CriarServico()
        {
            return new FaturaService(_mockFaturaRepo.Object, _mockClienteRepo.Object);
        }

        [Fact]
        public async Task AddAsync_Deve_Adicionar_Quando_Cliente_Existe()
        {
            var clienteId = 1;
            _mockClienteRepo.Setup(c => c.GetByIdAsync(clienteId))
                            .ReturnsAsync(new Cliente { ClienteId = clienteId, Nome = "Cliente Teste" });

            var service = CriarServico();

            var dto = new FaturaCreateDTO {
                ClienteId = clienteId,
                Data = DateTime.Now
            };

            await service.AddAsync(dto);

            _mockFaturaRepo.Verify(r => r.AddAsync(It.Is<Fatura>(f => f.ClienteId == clienteId)), Times.Once);
        }

        [Fact]
        public async Task AddAsync_Deve_Lancar_Exception_Quando_Cliente_Nao_Existe()
        {
            _mockClienteRepo.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                            .ReturnsAsync((Cliente)null);

            var service = CriarServico();

            var dto = new FaturaCreateDTO {
                ClienteId = 999,
                Data = DateTime.Now
            };

            var ex = await Assert.ThrowsAsync<Exception>(() => service.AddAsync(dto));
            Assert.Contains("Cliente com ID", ex.Message);
            _mockFaturaRepo.Verify(r => r.AddAsync(It.IsAny<Fatura>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_Deve_Excluir_Quando_Fatura_Existe()
        {
            var faturaId = 1;
            _mockFaturaRepo.Setup(r => r.GetByIdAsync(faturaId))
                           .ReturnsAsync(new Fatura { FaturaId = faturaId });

            var service = CriarServico();

            await service.DeleteAsync(faturaId);

            _mockFaturaRepo.Verify(r => r.DeleteAsync(faturaId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Deve_Lancar_Exception_Quando_Fatura_Nao_Existe()
        {
            _mockFaturaRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Fatura)null);

            var service = CriarServico();

            var ex = await Assert.ThrowsAsync<Exception>(() => service.DeleteAsync(999));
            Assert.Contains("Fatura com ID", ex.Message);
            _mockFaturaRepo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetAllAsync_Deve_Retornar_FaturaDTOs_SomenteComCamposBasicos()
        {
            var faturas = new List<Fatura>
            {
        new Fatura
        {
            FaturaId = 1,
            Data = new DateTime(2025, 8, 8),
            Cliente = new Cliente { ClienteId = 1, Nome = "Cliente 1" }, 
            FaturaItem = new List<FaturaItem>
            {
                new FaturaItem { Valor = 100 },
                new FaturaItem { Valor = 50 }
            }
        }
    };

            _mockFaturaRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(faturas);
            var service = CriarServico();

            var result = (await service.GetAllAsync()).ToList();

            Assert.Single(result);
            Assert.Equal(1, result[0].FaturaId);
            Assert.Equal(new DateTime(2025, 8, 8), result[0].Data);
            Assert.Null(result[0].Cliente);
            Assert.Empty(result[0].Itens);   
            Assert.Equal(0, result[0].Total);
        }

        [Fact]
        public async Task GetAllWithDetailsAsync_Deve_Retornar_FaturaDTOs_ComDetalhes()
        {

            var faturas = new List<Fatura>
            {
        new Fatura
        {
            FaturaId = 1,
            Data = new DateTime(2025, 1, 1),
            Cliente = new Cliente { ClienteId = 1, Nome = "Cliente 1" },
            FaturaItem = new List<FaturaItem>
            {
                new FaturaItem { Valor = 100 },
                new FaturaItem { Valor = 50 }
            }
        }
    };

            _mockFaturaRepo.Setup(r => r.GetAllWithDetailsAsync()).ReturnsAsync(faturas);
            var service = CriarServico();

            var result = (await service.GetAllWithDetailsAsync()).ToList();

            Assert.Single(result);
            var faturaDto = result[0];

            Assert.Equal(1, faturaDto.FaturaId);
            Assert.Equal(new DateTime(2025, 1, 1), faturaDto.Data);
            Assert.NotNull(faturaDto.Cliente);
            Assert.Equal(1, faturaDto.Cliente.ClienteId);
            Assert.Equal("Cliente 1", faturaDto.Cliente.Nome);
            Assert.Equal(150, faturaDto.Total);
            Assert.NotNull(faturaDto.Itens);
            Assert.Empty(faturaDto.Itens); 
        }

        [Fact]
        public async Task GetAllByClienteIdAsync_Deve_Retornar_Faturas_Quando_Cliente_Existe()
        {
            int clienteId = 1;

            _mockClienteRepo.Setup(c => c.GetByIdAsync(clienteId))
                            .ReturnsAsync(new Cliente { ClienteId = clienteId, Nome = "Cliente Teste" });

            var faturas = new List<Fatura>
            {
            new Fatura
            {
                FaturaId = 1,
                Cliente = new Cliente { ClienteId = clienteId, Nome = "Cliente Teste" },
                Data = DateTime.Now,
                FaturaItem = new List<FaturaItem>
                {
                    new FaturaItem { Valor = 100, Ordem = 10, Descricao = "Item 1" }
                }
            }
        };

            _mockFaturaRepo.Setup(r => r.GetByClienteIdAsync(clienteId)).ReturnsAsync(faturas);

            var service = CriarServico();

            var result = await service.GetAllByClienteIdAsync(clienteId);

            Assert.Single(result);
            Assert.Equal(clienteId, result.First().Cliente.ClienteId);
            Assert.Equal(100, result.First().Total);
        }

        [Fact]
        public async Task GetAllByClienteIdAsync_Deve_Lancar_Exception_Quando_Cliente_Nao_Existe()
        {
            _mockClienteRepo.Setup(c => c.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Cliente)null);

            var service = CriarServico();

            var ex = await Assert.ThrowsAsync<Exception>(() => service.GetAllByClienteIdAsync(999));
            Assert.Contains("Cliente com ID", ex.Message);
        }

        [Fact]
        public async Task GetByIdAsync_Deve_Retornar_Fatura_Quando_Existe()
        {
            var faturaId = 1;
            var fatura = new Fatura {
                FaturaId = faturaId,
                Data = DateTime.Today,
                Cliente = new Cliente { ClienteId = 1, Nome = "Cliente Teste" },
                FaturaItem = new List<FaturaItem>
                {
            new FaturaItem { FaturaItemId = 1, Descricao = "Item 1", Valor = 100, Ordem = 10 },
            new FaturaItem { FaturaItemId = 2, Descricao = "Item 2", Valor = 200, Ordem = 20 }
        }
            };

            _mockFaturaRepo.Setup(r => r.GetByIdAsync(faturaId)).ReturnsAsync(fatura);

            var service = CriarServico();

            var result = await service.GetByIdAsync(faturaId);

            Assert.NotNull(result);
            Assert.Equal(faturaId, result.FaturaId);
            Assert.Equal("Cliente Teste", result.Cliente.Nome);
            Assert.Equal(2, result.Itens.Count);
            Assert.Equal(300, result.Total);
        }

        [Fact]
        public async Task GetByIdAsync_Deve_Lancar_Exception_Quando_Fatura_Nao_Existe()
        {
            _mockFaturaRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Fatura)null);

            var service = CriarServico();

            var ex = await Assert.ThrowsAsync<Exception>(() => service.GetByIdAsync(999));
            Assert.Contains("Fatura não encontrada", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_Deve_Atualizar_Quando_Fatura_E_Cliente_Existem()
        {
            var faturaId = 1;
            var clienteId = 1;

            var faturaExistente = new Fatura {
                FaturaId = faturaId,
                ClienteId = clienteId,
                Data = DateTime.Today
            };

            _mockFaturaRepo.Setup(r => r.GetByIdWithDetailsAsync(faturaId)).ReturnsAsync(faturaExistente);
            _mockClienteRepo.Setup(c => c.GetByIdAsync(clienteId)).ReturnsAsync(new Cliente { ClienteId = clienteId });

            var service = CriarServico();

            var dto = new FaturaUpdateDTO {
                FaturaId = faturaId,
                ClienteId = clienteId,
                Data = DateTime.Today.AddDays(1)
            };

            await service.UpdateAsync(dto);

            _mockFaturaRepo.Verify(r => r.UpdateAsync(It.Is<Fatura>(f => f.FaturaId == faturaId && f.Data == dto.Data)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Deve_Lancar_Exception_Quando_Fatura_Nao_Existe()
        {
            _mockFaturaRepo.Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<int>())).ReturnsAsync((Fatura)null);

            var service = CriarServico();

            var dto = new FaturaUpdateDTO {
                FaturaId = 999,
                ClienteId = 1,
                Data = DateTime.Today
            };

            var ex = await Assert.ThrowsAsync<Exception>(() => service.UpdateAsync(dto));
            Assert.Contains("Fatura com ID", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_Deve_Lancar_Exception_Quando_Cliente_Novo_Nao_Existe()
        {
            var faturaId = 1;
            var clienteIdAtual = 1;
            var clienteNovoId = 2;

            var faturaExistente = new Fatura {
                FaturaId = faturaId,
                ClienteId = clienteIdAtual,
                Data = DateTime.Today
            };

            _mockFaturaRepo.Setup(r => r.GetByIdWithDetailsAsync(faturaId)).ReturnsAsync(faturaExistente);
            _mockClienteRepo.Setup(c => c.GetByIdAsync(clienteNovoId)).ReturnsAsync((Cliente)null);

            var service = CriarServico();

            var dto = new FaturaUpdateDTO {
                FaturaId = faturaId,
                ClienteId = clienteNovoId,
                Data = DateTime.Today
            };

            var ex = await Assert.ThrowsAsync<Exception>(() => service.UpdateAsync(dto));
            Assert.Contains("Novo Cliente com ID", ex.Message);
        }
    }
}

