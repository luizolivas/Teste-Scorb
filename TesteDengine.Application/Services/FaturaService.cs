using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services.Interfaces;
using TesteDengine.Domain.Interfaces;
using TesteDengine.Infrastructure.Repositories;

namespace TesteDengine.Application.Services
{
    public class FaturaService : IFaturaService
    {
        private readonly IFaturaRepository _faturaRepository;
        private readonly IClienteRepository _clienteRepository;
        
        public FaturaService(IFaturaRepository repository, IClienteRepository clienteRepository)
        {
            _faturaRepository = repository;
            _clienteRepository = clienteRepository;
        }

        public async Task AddAsync(FaturaCreateDTO dto)
        {
            var cliente = await _clienteRepository.GetByIdAsync(dto.ClienteId);
            if (cliente == null)
            {
                throw new Exception($"Cliente com ID {dto.ClienteId} não encontrado.");
            }

            var novaFatura = new Fatura {
                ClienteId = dto.ClienteId,
                Data = dto.Data,
                FaturaItem = new List<FaturaItem>() 
            };

            await _faturaRepository.AddAsync(novaFatura);
        }

        public async Task DeleteAsync(int id)
        {
            var faturaToDelete = await _faturaRepository.GetByIdAsync(id);
            if (faturaToDelete == null)
            {
                throw new Exception($"Fatura com ID {id} não encontrada.");
            }


            await _faturaRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<FaturaDTO>> GetAllAsync()
        {
            var faturas = await _faturaRepository.GetAllAsync(); 

            var faturaDTOs = faturas.Select(fatura => new FaturaDTO {
                FaturaId = fatura.FaturaId,
                Data = fatura.Data
            }).ToList();

            return faturaDTOs;
        }

        public async Task<IEnumerable<FaturaDTO>> GetAllWithDetailsAsync()
        {
            var faturas = await _faturaRepository.GetAllWithDetailsAsync();

            var faturaDTOs = faturas.Select(fatura => new FaturaDTO {
                FaturaId = fatura.FaturaId,
                Data = fatura.Data,
                Cliente = new ClienteDTO {
                    ClienteId = fatura.Cliente.ClienteId,
                    Nome = fatura.Cliente.Nome
                },
                Itens = new List<FaturaItemDTO>(),
                Total = fatura.FaturaItem?.Sum(i => i.Valor) ?? 0
            }).ToList();

            return faturaDTOs;
        }

        public async Task<IEnumerable<FaturaDTO>> GetAllByClienteIdAsync(int idCliente)
        {
            var cliente = await _clienteRepository.GetByIdAsync(idCliente);
            if (cliente == null)
            {
                throw new Exception($"Cliente com ID {idCliente} não encontrado.");
            }

            var faturas = await _faturaRepository.GetByClienteIdAsync(idCliente); 

            var faturaDTOs = faturas.Select(fatura => new FaturaDTO {
                FaturaId = fatura.FaturaId,
                Data = fatura.Data,
                Cliente = new ClienteDTO {
                    ClienteId = fatura.Cliente.ClienteId,
                    Nome = fatura.Cliente.Nome
                },
                Itens = fatura.FaturaItem.Select(i => new FaturaItemDTO {
                    FaturaItemId = i.FaturaItemId,
                    Descricao = i.Descricao,
                    Valor = i.Valor,
                    Ordem = i.Ordem,
                }).ToList(),
                Total = fatura.FaturaItem.Sum(i => i.Valor)
            }).ToList();

            return faturaDTOs;
        }

        public async Task<FaturaDTO?> GetByIdAsync(int id)
        {
            var fatura = await _faturaRepository.GetByIdAsync(id);

            if (fatura == null)
                throw new Exception("Fatura não encontrada.");

            return new FaturaDTO {
                FaturaId = fatura.FaturaId,
                Data = fatura.Data,
                Cliente = new ClienteDTO {
                    ClienteId = fatura.Cliente.ClienteId,
                    Nome = fatura.Cliente.Nome
                },
                Itens = fatura.FaturaItem.Select(i => new FaturaItemDTO {
                    FaturaItemId = i.FaturaItemId,
                    Descricao = i.Descricao,
                    Valor = i.Valor,
                    Ordem = i.Ordem,
                }).ToList() ?? new List<FaturaItemDTO>(),
                Total = fatura.FaturaItem.Sum(i => i.Valor)
            };
        }

        public async Task<FaturaDTO?> GetByIdWithDetailsAsync(int id)
        {
            var fatura = await _faturaRepository.GetByIdWithDetailsAsync(id);

            if (fatura == null)
                throw new Exception("Fatura não encontrada.");

            return new FaturaDTO {
                FaturaId = fatura.FaturaId,
                Data = fatura.Data,
                Cliente = new ClienteDTO {
                    ClienteId = fatura.Cliente.ClienteId,
                    Nome = fatura.Cliente.Nome
                },
                Itens = fatura.FaturaItem.Select(i => new FaturaItemDTO {
                    FaturaItemId = i.FaturaItemId,
                    Descricao = i.Descricao,
                    Valor = i.Valor,
                    Ordem = i.Ordem
                }).ToList(),

                Total = fatura.FaturaItem.Sum(i => i.Valor)
            };
        }

        public async Task UpdateAsync(FaturaUpdateDTO dto)
        {
            var faturaExistente = await _faturaRepository.GetByIdWithDetailsAsync(dto.FaturaId);
            if (faturaExistente == null)
            {
                throw new Exception($"Fatura com ID {dto.FaturaId} não encontrada para atualização.");
            }

            if (faturaExistente.ClienteId != dto.ClienteId)
            {
                var novoCliente = await _clienteRepository.GetByIdAsync(dto.ClienteId);
                if (novoCliente == null)
                {
                    throw new Exception($"Novo Cliente com ID {dto.ClienteId} não encontrado.");
                }
            }

            faturaExistente.ClienteId = dto.ClienteId;
            faturaExistente.Data = dto.Data;

            await _faturaRepository.UpdateAsync(faturaExistente);
        }
    }
}
