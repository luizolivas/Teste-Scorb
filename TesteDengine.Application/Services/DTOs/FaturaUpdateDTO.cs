using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteDengine.Application.Services.DTOs
{
    public class FaturaUpdateDTO
    {
        public int FaturaId { get; set; }
        public int ClienteId { get; set; }
        public DateTime Data { get; set; }
    }
}
