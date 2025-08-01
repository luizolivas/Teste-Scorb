using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services.Interfaces;
using TesteDengine.Domain.Entities;
using TesteDengine.Domain.Interfaces;

namespace TesteDengine.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IFaturaRepository _faturaRepository;

        public ClienteService(IClienteRepository clienteRepository, IFaturaRepository faturaRepository)
        {
            _clienteRepository = clienteRepository;
            _faturaRepository = faturaRepository;
        }

        public async Task AddAsync(ClienteCreateDTO dto)
        {
            var novoCliente = new Cliente {
                Nome = dto.Nome,
            };

            await _clienteRepository.AddAsync(novoCliente);
        }

        public async Task DeleteAsync(int id)
        {
            var clienteToDelete = await _clienteRepository.GetByIdAsync(id);
            if (clienteToDelete == null)
            {
                throw new Exception($"Cliente com ID {id} não encontrado para exclusão.");
            }

            var fatura = await _faturaRepository.GetByClienteIdAsync(clienteToDelete.ClienteId);

            if(fatura != null)
            {
                throw new Exception("Não é possivel excluir um cliente enquanto há faturas cadastradas em seu nome!");
            }

            await _clienteRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ClienteDTO>> GetAllAsync()
        {
            var clientes = await _clienteRepository.GetAllAsync();

            var clienteDTOs = clientes.Select(cliente => new ClienteDTO {
                ClienteId = cliente.ClienteId,
                Nome = cliente.Nome
            }).ToList();

            return clienteDTOs;
        }

        public async Task<ClienteDTO?> GetByIdAsync(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);

            if (cliente == null)
            {
                return null; 
            }

            return new ClienteDTO {
                ClienteId = cliente.ClienteId,
                Nome = cliente.Nome
            };
        }

        public async Task UpdateAsync(ClienteUpdateDTO dto)
        {
            var clienteExistente = await _clienteRepository.GetByIdAsync(dto.ClienteId);
            if (clienteExistente == null)
            {
                throw new Exception($"Cliente com ID {dto.ClienteId} não encontrado para atualização.");
            }

            clienteExistente.Nome = dto.Nome;

            await _clienteRepository.UpdateAsync(clienteExistente);
        }
    }
}
