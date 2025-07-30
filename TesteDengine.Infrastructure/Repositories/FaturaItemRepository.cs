using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Domain.Interfaces;

namespace TesteDengine.Infrastructure.Repositories
{
    public class FaturaItemRepository : IFaturaItemItemRepository
    {
        private readonly DbExercicio4 _context;
        public FaturaItemRepository(DbExercicio4 context) 
        {
            _context = context;
        }
        public Task<FaturaItem?> AddAsync(FaturaItem food)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FaturaItem>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<FaturaItem?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(FaturaItem food)
        {
            throw new NotImplementedException();
        }
    }
}
