using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteDengine.Domain.Entities
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; } 
        public virtual ICollection<Fatura> Faturas { get; set; } = new HashSet<Fatura>();
    }
}
