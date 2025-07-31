using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Domain.Interfaces;

namespace TesteDengine.Infrastructure.Repositories
{
    public class FaturaItemRepository : IFaturaItemRepository
    {
        private readonly DbExercicio4 _context;
        public FaturaItemRepository(DbExercicio4 context) 
        {
            _context = context;
        }
        public async Task<FaturaItem?> AddAsync(FaturaItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item da fatura não pode ser nulo.");
            }

            await _context.FaturaItem.AddAsync(item);
            await _context.SaveChangesAsync(); 
            return item;
        }

        public async Task DeleteAsync(int id)
        {
            var itemToDelete = await _context.FaturaItem.FindAsync(id); 
            if (itemToDelete != null)
            {
                _context.FaturaItem.Remove(itemToDelete); 
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task<IEnumerable<FaturaItem>> GetAllAsync()
        {
            return await _context.FaturaItem.ToListAsync();
        }

        public async Task<IEnumerable<FaturaItem>> GetByFaturaIdAsync(int faturaId)
        {
            return await _context.FaturaItem
                .Where(item => item.FaturaId == faturaId)
                .ToListAsync();
        }

        public async Task<FaturaItem?> GetByIdAsync(int id)
        {
            return await _context.FaturaItem.FindAsync(id);
        }

        public async Task UpdateAsync(FaturaItem fatura)
        {
            if (fatura == null)
            {
                throw new ArgumentNullException(nameof(fatura), "Item da fatura não pode ser nulo.");
            }

            _context.FaturaItem.Update(fatura);
            await _context.SaveChangesAsync();
        }
    }
}
