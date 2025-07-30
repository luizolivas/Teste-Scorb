using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteDengine.Application.Services.DTOs
{
    public class FaturaItemUpdateDTO
    {
        public int FaturaItemId { get; set; }
        public int Ordem { get; set; }
        public double Valor { get; set; }
        public string Descricao { get; set; }
    }
}
