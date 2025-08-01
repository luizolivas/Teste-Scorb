using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Application.Services.DTOs;

namespace TesteDengine.Application.Services.Interfaces
{
    public interface IFaturaItemService
    {
        Task<IEnumerable<FaturaItemDTO>> GetAllAsync();
        Task<IEnumerable<FaturaItemDTO>> GetAllByFaturaIdAsync(int faturaId);
        Task<FaturaItemDTO?> GetByIdAsync(int id);
        Task<IEnumerable<FaturaItemDTO>> GetByFaturaIdAsync(int faturaId);
        Task AddAsync(FaturaItemCreateDTO dto);
        Task UpdateAsync(FaturaItemUpdateDTO dto);
        Task DeleteAsync(int id);

    }
}
