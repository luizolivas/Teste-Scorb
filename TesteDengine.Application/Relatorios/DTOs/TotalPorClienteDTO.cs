using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteDengine.Application.Relatorios.DTOs
{
    public class TotalPorClienteDTO
    {
        public int ClienteId { get; set; } 
        public string ClienteNome { get; set; } 
        public double Total { get; set; }
    }
}
