using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Domain.Entities;

namespace TesteDengine.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByIdAsync(int id);
        Task<Cliente?> AddAsync(Cliente food);
        Task UpdateAsync(Cliente food);
        Task DeleteAsync(int id);
    }
}
