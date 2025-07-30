using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Domain.Interfaces;

namespace TesteDengine.Infrastructure.Repositories
{
    public class FaturaRepository : IFaturaRepository
    {
        public Task<Fatura?> AddAsync(Fatura food)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Fatura>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Fatura?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Fatura food)
        {
            throw new NotImplementedException();
        }
    }
}
