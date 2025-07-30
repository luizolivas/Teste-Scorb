using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Domain.Entities;
using TesteDengine.Domain.Interfaces;

namespace TesteDengine.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        public Task<Cliente?> AddAsync(Cliente food)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cliente>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Cliente?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Cliente food)
        {
            throw new NotImplementedException();
        }
    }
}
