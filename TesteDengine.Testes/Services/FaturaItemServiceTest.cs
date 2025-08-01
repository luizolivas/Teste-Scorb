using System.ComponentModel.DataAnnotations;
using Moq;
using Xunit;
using TesteDengine.Application.Services;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Domain.Entities;
using TesteDengine.Domain.Interfaces;
using TesteDengine;

namespace TesteDengine.Testes.Services
{
    public class FaturaItemServiceTest
    {
        private readonly Mock<IFaturaItemRepository> _itemRepo = new();
        private readonly Mock<IFaturaRepository> _faturaRepo = new();
        private readonly ValidadorFaturaItem _validador = new();

        private FaturaItemService CriarService()
            => new FaturaItemService(_itemRepo.Object, _faturaRepo.Object, _validador);

        [Fact]
        public async Task AddAsync_DeveAdicionarItemQuandoValido()
        {
            var dto = new FaturaItemCreateDTO {
                FaturaId = 1,
                Ordem = 10,
                Valor = 100,
                Descricao = "Item válido"
            };

            _faturaRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Fatura { FaturaId = 1 });
            _itemRepo.Setup(r => r.GetByFaturaIdAsync(1)).ReturnsAsync(new List<FaturaItem>());

            var service = CriarService();
            await service.AddAsync(dto);

            _itemRepo.Verify(r => r.AddAsync(It.IsAny<FaturaItem>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_DeveLancarExcecao_QuandoFaturaNaoExiste()
        {
            var dto = new FaturaItemCreateDTO { FaturaId = 99, Ordem = 1, Valor = 10, Descricao = "Teste" };
            _faturaRepo.Setup(r => r.GetByIdAsync(dto.FaturaId)).ReturnsAsync((Fatura?)null);

            var service = CriarService();

            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.AddAsync(dto));
            Assert.Contains("Fatura com ID 99", ex.Message);
        }

        [Fact]
        public async Task DeleteAsync_DeveLancarExcecao_SeNaoExistir()
        {
            _itemRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((FaturaItem?)null);

            var service = CriarService();
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(1));
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarItens()
        {
            _itemRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<FaturaItem> {
            new FaturaItem { FaturaItemId = 1, Descricao = "Teste", Valor = 10, Ordem = 1, FaturaId = 1 }
        });

            var service = CriarService();
            var result = await service.GetAllAsync();

            Assert.Single(result);
            Assert.Equal("Teste", result.First().Descricao);
        }

        [Fact]
        public async Task UpdateAsync_DeveAtualizarSeValido()
        {
            var dto = new FaturaItemUpdateDTO { FaturaItemId = 1, Ordem = 10, Valor = 100, Descricao = "Atualizado" };

            _itemRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new FaturaItem {
                FaturaItemId = 1,
                FaturaId = 1,
                Ordem = 10,
                Valor = 50,
                Descricao = "Original"
            });

            _itemRepo.Setup(r => r.GetByFaturaIdAsync(1)).ReturnsAsync(new List<FaturaItem> {
            new FaturaItem { FaturaItemId = 1, FaturaId = 1, Ordem = 10, Valor = 50 }
        });

            var service = CriarService();
            await service.UpdateAsync(dto);

            _itemRepo.Verify(r => r.UpdateAsync(It.Is<FaturaItem>(f => f.Ordem == 10 && f.Descricao == "Atualizado")));
        }

        [Fact]
        public async Task UpdateAsync_DeveLancarExcecao_SeItemNaoExiste()
        {
            _itemRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((FaturaItem?)null);

            var dto = new FaturaItemUpdateDTO { FaturaItemId = 1, Ordem = 10, Valor = 100, Descricao = "Novo" };

            var service = CriarService();
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateAsync(dto));
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNulo_SeNaoExiste()
        {
            _itemRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((FaturaItem?)null);

            var service = CriarService();
            var result = await service.GetByIdAsync(10);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_DeveLancarExcecao_SeOrdemNaoMultiploDe10()
        {
            var itemExistente = new FaturaItem { FaturaItemId = 1, FaturaId = 100, Ordem = 10 };
            var dto = new FaturaItemUpdateDTO { FaturaItemId = 1, Ordem = 15, Valor = 100, Descricao = "Atualizado" };

            _itemRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(itemExistente);
            _itemRepo.Setup(r => r.GetByFaturaIdAsync(100)).ReturnsAsync(new List<FaturaItem> { itemExistente });

            var service = CriarService();

            var ex = await Assert.ThrowsAsync<Exception>(() => service.UpdateAsync(dto));
            Assert.Contains("a ordem deve ser múltiplo de 10.", ex.Message.ToLower());
        }

        [Fact]
        public async Task UpdateAsync_DeveLancarExcecao_SeOrdemDuplicada()
        {
            var item1 = new FaturaItem { FaturaItemId = 1, FaturaId = 100, Ordem = 10 };
            var item2 = new FaturaItem { FaturaItemId = 2, FaturaId = 100, Ordem = 20 };

            var dto = new FaturaItemUpdateDTO { FaturaItemId = 1, Ordem = 20, Valor = 100, Descricao = "Atualizado" };

            _itemRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(item1);
            _itemRepo.Setup(r => r.GetByFaturaIdAsync(100)).ReturnsAsync(new List<FaturaItem> { item1, item2 });

            var service = CriarService();

            var ex = await Assert.ThrowsAsync<ValidationException>(() => service.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_DeveLancarExcecao_SeHouverBuracoNaOrdem()
        {
            var item1 = new FaturaItem { FaturaItemId = 1, FaturaId = 100, Ordem = 10 };
            var item2 = new FaturaItem { FaturaItemId = 2, FaturaId = 100, Ordem = 30 };

            var dto = new FaturaItemUpdateDTO { FaturaItemId = 1, Ordem = 10, Valor = 100, Descricao = "Atualizado" };

            _itemRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(item1);
            _itemRepo.Setup(r => r.GetByFaturaIdAsync(100)).ReturnsAsync(new List<FaturaItem> { item1, item2 });

            var service = CriarService();

            var ex = await Assert.ThrowsAsync<ValidationException>(() => service.UpdateAsync(dto));
        }
    }
}

