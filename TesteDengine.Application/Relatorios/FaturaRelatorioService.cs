using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Application.Relatorios.DTOs;
using TesteDengine.Application.Relatorios.Interfaces;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Domain.Interfaces;

namespace TesteDengine.Application.Relatorios
{
    public class FaturaRelatorioService : IFaturaRelatorioService
    {
        private readonly IFaturaRepository _faturaRepository;
        private readonly IFaturaItemRepository _itemRepository;

        public FaturaRelatorioService(IFaturaRepository faturaRepository, IFaturaItemRepository itemRepository)
        {
            _faturaRepository = faturaRepository;
            _itemRepository = itemRepository;
        }

        public async Task<List<FaturaDTO>> ObterTop10FaturasAsync()
        {
            var faturas = await _faturaRepository.GetAllWithDetailsAsync();

            if (faturas == null || !faturas.Any())
                throw new KeyNotFoundException("Nenhum registro encontrado.");

            var top10Faturas = faturas
                .OrderByDescending(f => f.FaturaItem.Sum(fi => fi.Valor))
                .Take(10)
                .Select(f => new FaturaDTO {
                    FaturaId = f.FaturaId,
                    Data = f.Data,
                    Cliente = new ClienteDTO {
                        ClienteId = f.Cliente.ClienteId,
                        Nome = f.Cliente.Nome
                    },
                    Itens = f.FaturaItem.Select(i => new FaturaItemDTO {
                        FaturaItemId = i.FaturaItemId,
                        Descricao = i.Descricao,
                        Valor = i.Valor,
                        Ordem = i.Ordem
                    }).ToList(),
                    Total = f.FaturaItem.Sum(i => i.Valor)
                })
                .ToList();

            return top10Faturas;
        }

        public async Task<List<FaturaItemDTO>> ObterTop10ItensAsync()
        {
            var itensFatura = await _itemRepository.GetAllAsync();

            if (itensFatura == null || !itensFatura.Any())
                throw new KeyNotFoundException("Nenhum registro encontrado.");

            var top10Itens = itensFatura
                .OrderByDescending(fi => fi.Valor)
                .Take(10)
                 .Select(fi => new FaturaItemDTO { 
                    FaturaItemId = fi.FaturaItemId,
                    Ordem = fi.Ordem,
                    Descricao = fi.Descricao,
                    Valor = fi.Valor
                 })
                 .ToList();

            return top10Itens;
        }

        public async Task<List<TotalPorClienteDTO>> ObterTotalPorClienteAsync()
        {
            var faturas = await _faturaRepository.GetAllAsync();

            if (faturas == null || !faturas.Any())
                throw new KeyNotFoundException("Nenhum registro encontrado.");

            var totalPorCliente = faturas
                .GroupBy(f => new { f.Cliente.ClienteId, f.Cliente.Nome })
                .Select(g => new TotalPorClienteDTO {
                    ClienteId = g.Key.ClienteId,
                    ClienteNome = g.Key.Nome,
                    Total = g.Sum(f => f.FaturaItem.Sum(i => i.Valor))
                })
                .ToList();

            return totalPorCliente;
        }

        public async Task<List<TotalPorMesAnoDTO>> ObterTotalPorMesAnoAsync()
        {
            var faturas = await _faturaRepository.GetAllWithDetailsAsync();

            if (faturas == null || !faturas.Any())
                throw new KeyNotFoundException("Nenhum registro encontrado.");

            var totalMesAno = faturas
                .GroupBy(f => new {Ano = f.Data.Year, Mes = f.Data.Month})
                .Select(r => new TotalPorMesAnoDTO {
                    Ano = r.Key.Ano,
                    Mes = r.Key.Mes,
                    Total = r.Sum(f => f.FaturaItem.Sum(i => i.Valor))
                })
                .OrderBy(f => f.Ano)
                .ThenBy(f => f.Mes)
                .ToList();

            return totalMesAno;
        }
    }
}
