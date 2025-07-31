using Microsoft.EntityFrameworkCore;
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
        private readonly DbExercicio4 _context;

        public ClienteRepository(DbExercicio4 context)
        {
            _context = context;
        }

        public async Task<Cliente?> AddAsync(Cliente cliente) 
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "Cliente não pode ser nulo.");
            }

            await _context.Cliente.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return cliente; 
        }

        public async Task DeleteAsync(int id)
        {
            var clienteToDelete = await _context.Cliente.FindAsync(id); 
            if (clienteToDelete != null)
            {
                _context.Cliente.Remove(clienteToDelete); 
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Cliente.ToListAsync(); 
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Cliente.FindAsync(id);
        }

        public async Task UpdateAsync(Cliente cliente) 
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "Cliente não pode ser nulo.");
            }

            _context.Cliente.Update(cliente);
            await _context.SaveChangesAsync(); 
        }
    }
}
