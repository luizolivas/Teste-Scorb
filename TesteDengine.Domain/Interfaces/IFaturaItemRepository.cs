using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteDengine.Domain.Interfaces
{
    public interface IFaturaItemRepository
    {
        Task<IEnumerable<FaturaItem>> GetAllAsync();
        Task<FaturaItem?> GetByIdAsync(int id);
        Task<IEnumerable<FaturaItem>> GetByFaturaIdAsync(int id);
        Task<FaturaItem?> AddAsync(FaturaItem food);
        Task UpdateAsync(FaturaItem food);
        Task DeleteAsync(int id);
    }
}
