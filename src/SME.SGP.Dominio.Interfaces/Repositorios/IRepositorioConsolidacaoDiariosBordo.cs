using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoDiariosBordo
    {
        Task<IEnumerable<ConsolidacaoDiariosBordo>> GerarConsolidacaoPorUe(long ueId, int anoLetivo);
        Task Salvar(ConsolidacaoDiariosBordo entidade);
        Task ExcluirPorAno(int anoLetivo);

        Task<IEnumerable<GraficoTotalDiariosEDevolutivasPorDreDTO>> ObterQuantidadeTotalDeDiariosPendentesPorDre(int anoLetivo, string ano);
    }
}
