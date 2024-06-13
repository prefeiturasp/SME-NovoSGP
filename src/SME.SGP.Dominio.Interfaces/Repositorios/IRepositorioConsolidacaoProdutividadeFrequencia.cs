using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoProdutividadeFrequencia : IRepositorioBase<ConsolidacaoProdutividadeFrequencia>
    {
        Task ExcluirConsolidacoes(string ueCodigo, int anoLetivo);
    }
}
