using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services.Interfaces;
using TesteDengine.Domain.Interfaces;

namespace TesteDengine.Application.Services
{
    public class FaturaItemService : IFaturaItemService
    {
        private readonly IFaturaItemRepository _faturaItemRepository;
        private readonly IFaturaRepository _faturaRepository;
        private readonly ValidadorFaturaItem validadorFaturaItem;

        public FaturaItemService(IFaturaItemRepository faturaItemRepository, IFaturaRepository faturaRepository, ValidadorFaturaItem validadorFaturaItem)
        {
            _faturaItemRepository = faturaItemRepository;
            _faturaRepository = faturaRepository;
            this.validadorFaturaItem = validadorFaturaItem;
        }

        public async Task AddAsync(FaturaItemCreateDTO dto)
        {
            if (dto.FaturaId == 0) 
            {
                throw new ArgumentException("O FaturaId deve ser fornecido para o item.");
            }

            var novoFaturaItem = new FaturaItem {
                FaturaId = dto.FaturaId,
                Ordem = dto.Ordem,
                Valor = dto.Valor,
                Descricao = dto.Descricao,
            };

            var existingItemsInFatura = await _faturaItemRepository.GetByFaturaIdAsync(dto.FaturaId);

            try
            {
                validadorFaturaItem.Validar(novoFaturaItem, existingItemsInFatura.ToList());
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha na validação do FaturaItem: {ex.Message}", ex);
            }

            await _faturaItemRepository.AddAsync(novoFaturaItem);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FaturaItemDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FaturaItemDTO>> GetAllByFaturaIdAsync(int faturaId)
        {
            var faturaItems = await _faturaItemRepository.GetByFaturaIdAsync(faturaId);
            return faturaItems.Select(i => new FaturaItemDTO {
                FaturaItemId = i.FaturaItemId,
                Descricao = i.Descricao,
                Valor = i.Valor,
                Ordem = i.Ordem
            }).ToList();
        }

        public Task<FaturaItemDTO?> GetByFaturaIdAsync(int faturaId)
        {
            throw new NotImplementedException();
        }

        public Task<FaturaItemDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(FaturaItemUpdateDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
