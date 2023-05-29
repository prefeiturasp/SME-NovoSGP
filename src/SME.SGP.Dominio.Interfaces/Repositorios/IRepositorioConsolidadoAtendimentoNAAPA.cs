using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidadoAtendimentoNAAPA : IRepositorioBase<ConsolidadoAtendimentoNAAPA>
    {
        Task<IEnumerable<ConsolidadoAtendimentoNAAPA>> ObterPorUeIdAnoLetivo(long ueId, int anoLetivo);
        Task<IEnumerable<QuantidadeEncaminhamentoNAAPAEmAbertoDto>> ObterQuantidadeEncaminhamentoNAAPAEmAberto(int anoLetivo, string codigoDre);
    }
}