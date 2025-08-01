using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteDengine.Application.Relatorios.DTOs;
using TesteDengine.Application.Services.DTOs;

namespace TesteDengine.Application.Relatorios.Interfaces
{
    public interface IFaturaRelatorioService
    {
        Task<List<TotalPorClienteDTO>> ObterTotalPorClienteAsync();
        Task<List<TotalPorMesAnoDTO>> ObterTotalPorMesAnoAsync();
        Task<List<FaturaDTO>> ObterTop10FaturasAsync();
        Task<List<FaturaItemDTO>> ObterTop10ItensAsync();
    }
}
