using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services.Interfaces;

namespace TesteDengine.Application.Services
{
    public class FaturaService : IFaturaService
    {
        public Task AddAsync(FaturaCreateDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FaturaDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FaturaDTO>> GetAllByClienteIdAsync(int idCliente)
        {
            throw new NotImplementedException();
        }

        public Task<FaturaDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(FaturaUpdateDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
