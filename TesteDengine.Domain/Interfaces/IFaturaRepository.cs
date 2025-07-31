using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteDengine.Domain.Interfaces
{
    public interface IFaturaRepository
    {
        Task<IEnumerable<Fatura>> GetAllAsync();
        Task<Fatura?> GetByIdAsync(int id);
        Task<Fatura?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Fatura>> GetByClienteIdAsync(int idCliente);
        Task<Fatura?> AddAsync(Fatura food);
        Task UpdateAsync(Fatura food);
        Task DeleteAsync(int id);

    }
}
