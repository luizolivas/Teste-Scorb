using Microsoft.EntityFrameworkCore;
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

        private readonly DbExercicio4 _context;

        public FaturaRepository(DbExercicio4 context)
        {
            _context = context;
        }

        public async Task<Fatura?> AddAsync(Fatura fatura) 
        {
            if (fatura == null)
            {
                throw new ArgumentNullException(nameof(fatura), "Fatura não pode ser nula.");
            }

            await _context.Fatura.AddAsync(fatura);
            await _context.SaveChangesAsync(); 
            return fatura; 
        }

        public async Task DeleteAsync(int id)
        {
            var faturaToDelete = await _context.Fatura.FindAsync(id);
            if (faturaToDelete != null)
            {
                _context.Fatura.Remove(faturaToDelete);
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task<IEnumerable<Fatura>> GetAllAsync()
        {
            return await _context.Fatura
                .Include(f => f.Cliente) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Fatura>> GetAllWithDetailsAsync()
        {
            return await _context.Fatura
                .Include(f => f.Cliente)
                .Include(f => f.FaturaItem)
                .ToListAsync();
        }

        public async Task<IEnumerable<Fatura>> GetByClienteIdAsync(int idCliente)
        {
            return await _context.Fatura
                .Include(f => f.Cliente)   
                .Include(f => f.FaturaItem) 
                .Where(f => f.ClienteId == idCliente) 
                .ToListAsync();             
        }

        public async Task<Fatura?> GetByIdAsync(int id)
        {
            return await _context.Fatura.FindAsync(id);
        }

        public async Task<Fatura?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Fatura
                .Include(f => f.Cliente)     
                .Include(f => f.FaturaItem)  
                .FirstOrDefaultAsync(f => f.FaturaId == id); 
        }

        public async Task UpdateAsync(Fatura fatura) 
        {
            if (fatura == null)
            {
                throw new ArgumentNullException(nameof(fatura), "Fatura não pode ser nula.");
            }

            _context.Fatura.Update(fatura);
            await _context.SaveChangesAsync();
        }
    }
}
