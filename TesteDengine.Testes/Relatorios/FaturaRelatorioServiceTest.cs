using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TesteDengine.Application.Services;
using TesteDengine.Domain.Interfaces;
using TesteDengine.Application.Relatorios;
using TesteDengine;
using TesteDengine.Domain.Entities;

namespace TesteDengine.Testes.Relatorios
{
    public class FaturaRelatorioServiceTest
    {
        private readonly Mock<IFaturaRepository> _mockFaturaRepository = new();
        private readonly Mock<IFaturaItemRepository> _mockFaturaItemRepository = new();

        private FaturaRelatorioService CriarServico()
        {
            return new FaturaRelatorioService(_mockFaturaRepository.Object, _mockFaturaItemRepository.Object);
        }

        [Fact]
        public async Task ObterTotalPorClienteAsync_Deve_Retornar_Total_Correto()
        {
            // Arrange: preparar dados simulados
            var faturas = new List<Fatura>
            {
            new Fatura {
                FaturaId = 1,
                Cliente = new Cliente { ClienteId = 1, Nome = "Cliente A" },
                FaturaItem = new List<FaturaItem> {
                    new FaturaItem { Valor = 100 },
                    new FaturaItem { Valor = 200 }
                }
            },
            new Fatura {
                FaturaId = 2,
                Cliente = new Cliente { ClienteId = 2, Nome = "Cliente B" },
                FaturaItem = new List<FaturaItem> {
                    new FaturaItem { Valor = 300 }
                }
            },
            new Fatura {
                FaturaId = 3,
                Cliente = new Cliente { ClienteId = 1, Nome = "Cliente A" },
                FaturaItem = new List<FaturaItem> {
                    new FaturaItem { Valor = 400 }
                }
            }
        };

            _mockFaturaRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(faturas);

            var service = CriarServico();

            // Act
            var resultado = await service.ObterTotalPorClienteAsync();

            // Assert
            Assert.Equal(2, resultado.Count); 
            var clienteA = resultado.FirstOrDefault(c => c.ClienteId == 1);
            Assert.NotNull(clienteA);
            Assert.Equal(700, clienteA.Total); 
            var clienteB = resultado.FirstOrDefault(c => c.ClienteId == 2);
            Assert.NotNull(clienteB);
            Assert.Equal(300, clienteB.Total);
        }

        [Fact]
        public async Task ObterTotalPorMesAnoAsync_Deve_Retornar_Total_Correto()
        {
            // Arrange
            var faturas = new List<Fatura>
            {
            new Fatura {
                FaturaId = 1,
                Data = new DateTime(2025, 7, 15),
                FaturaItem = new List<FaturaItem> {
                    new FaturaItem { Valor = 100 }
                }
            },
            new Fatura {
                FaturaId = 2,
                Data = new DateTime(2025, 7, 20),
                FaturaItem = new List<FaturaItem> {
                    new FaturaItem { Valor = 200 }
                }
            },
            new Fatura {
                FaturaId = 3,
                Data = new DateTime(2025, 8, 10),
                FaturaItem = new List<FaturaItem> {
                    new FaturaItem { Valor = 300 }
                }
            }
        };

            _mockFaturaRepository.Setup(r => r.GetAllWithDetailsAsync()).ReturnsAsync(faturas);

            var service = CriarServico();

            // Act
            var resultado = await service.ObterTotalPorMesAnoAsync();

            // Assert
            Assert.Equal(2, resultado.Count); 
            var jan = resultado.FirstOrDefault(x => x.Ano == 2025 && x.Mes == 7);
            Assert.NotNull(jan);
            Assert.Equal(300, jan.Total);
            var fev = resultado.FirstOrDefault(x => x.Ano == 2025 && x.Mes == 8);
            Assert.NotNull(fev);
            Assert.Equal(300, fev.Total);
        }

        [Fact]
        public async Task ObterTop10FaturasAsync_Deve_Retornar_As_10_Maiores()
        {
            // Arrange
            var faturas = Enumerable.Range(1, 15).Select(i => new Fatura {
                FaturaId = i,
                Data = DateTime.Now,
                Cliente = new Cliente { ClienteId = i, Nome = $"Cliente {i}" },
                FaturaItem = new List<FaturaItem>
                {
            new FaturaItem { Valor = i * 10 } // total crescente
        }
            }).ToList();

            _mockFaturaRepository.Setup(r => r.GetAllWithDetailsAsync()).ReturnsAsync(faturas);

            var service = CriarServico();

            // Act
            var resultado = await service.ObterTop10FaturasAsync();

            // Assert
            Assert.Equal(10, resultado.Count);
            Assert.Equal(150, resultado.First().Total); // maior valor (15 * 10)
            Assert.Equal(60, resultado.Last().Total);   // 6º menor no Top 10 (6 * 10)
        }

        [Fact]
        public async Task ObterTop10ItensAsync_Deve_Retornar_As_10_Maiores()
        {
            // Arrange
            var itens = Enumerable.Range(1, 20).Select(i => new FaturaItem {
                Descricao = $"Item {i}",
                Valor = i * 5
            }).ToList();

            _mockFaturaItemRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(itens);

            var service = CriarServico();

            // Act
            var resultado = await service.ObterTop10ItensAsync();

            // Assert
            Assert.Equal(10, resultado.Count);
            Assert.Equal("Item 20", resultado.First().Descricao); // maior valor
            Assert.Equal(100, resultado.First().Valor);
            Assert.Equal("Item 11", resultado.Last().Descricao); // 10ª maior
        }

    }

}

