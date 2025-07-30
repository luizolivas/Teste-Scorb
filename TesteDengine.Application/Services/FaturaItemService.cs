using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services.Interfaces;

namespace TesteDengine.Application.Services
{
    public class FaturaItemService : IFaturaItemService
    {
        public Task AddAsync(FaturaItemCreateDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FaturaItemDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FaturaItemDTO>> GetAllByFaturaIdAsync(int faturaId)
        {
            throw new NotImplementedException();
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
