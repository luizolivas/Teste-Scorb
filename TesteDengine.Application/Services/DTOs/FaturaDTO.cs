using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteDengine.Application.Services.DTOs
{
    public class FaturaDTO
    {
        public int FaturaId { get; set; }
        public ClienteDTO Cliente { get; set; }
        public DateTime Data { get; set; }
        public ICollection<FaturaItemDTO> Itens { get; set; } = new List<FaturaItemDTO>();
        public double Total { get; set; }
    }
}
