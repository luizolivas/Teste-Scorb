using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Application.Services.DTOs;

namespace TesteDengine.Application.Services.Interfaces
{
    public interface IFaturaService
    {
        Task<IEnumerable<FaturaDTO>> GetAllAsync();
        Task<IEnumerable<FaturaDTO>> GetAllWithDetailsAsync();
        Task<IEnumerable<FaturaDTO>> GetAllByClienteIdAsync(int idCliente);
        Task<FaturaDTO?> GetByIdAsync(int id);
        Task<FaturaDTO?> GetByIdWithDetailsAsync(int id);
        Task AddAsync(FaturaCreateDTO dto);
        Task UpdateAsync(FaturaUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}
