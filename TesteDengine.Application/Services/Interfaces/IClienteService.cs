using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Application.Services.DTOs;

namespace TesteDengine.Application.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDTO>> GetAllAsync();
        Task<ClienteDTO?> GetByIdAsync(int id);
        Task AddAsync(ClienteCreateDTO dto);
        Task UpdateAsync(ClienteUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}
