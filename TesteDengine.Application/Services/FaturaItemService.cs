using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var faturaExistente = await _faturaRepository.GetByIdAsync(dto.FaturaId);
            if (faturaExistente == null)
            {
                throw new KeyNotFoundException($"Fatura com ID {dto.FaturaId} não encontrada para adicionar item.");
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
            catch (ValidationException ex) 
            {
                throw new ValidationException($"Falha na validação das regras de negócio para o item da fatura: {ex.Message}", ex);
            }

            await _faturaItemRepository.AddAsync(novoFaturaItem);
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _faturaItemRepository.GetByIdAsync(id);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item da Fatura com ID {id} não encontrado para exclusão.");
            }

            await _faturaItemRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<FaturaItemDTO>> GetAllAsync()
        {
            var itens = await _faturaItemRepository.GetAllAsync();
            return itens.Select(i => new FaturaItemDTO {
                FaturaItemId = i.FaturaItemId,
                FaturaId = i.FaturaId,
                Ordem = i.Ordem,
                Valor = i.Valor,
                Descricao = i.Descricao
            });
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

        public async Task<IEnumerable<FaturaItemDTO>> GetByFaturaIdAsync(int faturaId)
        {
            var itens = await _faturaItemRepository.GetByFaturaIdAsync(faturaId);
            return itens.Select(i => new FaturaItemDTO {
                FaturaItemId = i.FaturaItemId,
                FaturaId = i.FaturaId,
                Ordem = i.Ordem,
                Valor = i.Valor,
                Descricao = i.Descricao
            });
        }

        public async Task<FaturaItemDTO?> GetByIdAsync(int id)
        {
            var item = await _faturaItemRepository.GetByIdAsync(id);
            if (item == null) return null;

            return new FaturaItemDTO {
                FaturaItemId = item.FaturaItemId,
                FaturaId = item.FaturaId,
                Ordem = item.Ordem,
                Valor = item.Valor,
                Descricao = item.Descricao
            };
        }

        public async Task UpdateAsync(FaturaItemUpdateDTO dto)
        {
            var itemExistente = await _faturaItemRepository.GetByIdAsync(dto.FaturaItemId);
            if (itemExistente == null)
            {
                throw new KeyNotFoundException($"Item da Fatura com ID {dto.FaturaItemId} não encontrado para atualização.");
            }

            var allItemsInFatura = await _faturaItemRepository.GetByFaturaIdAsync(itemExistente.FaturaId);
            var otherItemsInFatura = allItemsInFatura.Where(i => i.FaturaItemId != itemExistente.FaturaItemId).ToList();

            var itemAtualizado = new FaturaItem {
                FaturaItemId = itemExistente.FaturaItemId,
                FaturaId = itemExistente.FaturaId,
                Ordem = dto.Ordem,
                Valor = dto.Valor,
                Descricao = dto.Descricao
            };

            try
            {

                validadorFaturaItem.Validar(itemAtualizado, otherItemsInFatura);
            }
            catch (ValidationException ex)
            {
                throw new ValidationException($"Falha na validação das regras de negócio para o item da fatura: {ex.Message}", ex);
            }

            itemExistente.Ordem = dto.Ordem;
            itemExistente.Valor = dto.Valor;
            itemExistente.Descricao = dto.Descricao;
            await _faturaItemRepository.UpdateAsync(itemExistente);
        }
    }
}
