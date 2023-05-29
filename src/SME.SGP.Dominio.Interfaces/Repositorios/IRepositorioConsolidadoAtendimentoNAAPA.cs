using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidadoAtendimentoNAAPA : IRepositorioBase<ConsolidadoAtendimentoNAAPA>
    {
        Task<ConsolidadoAtendimentoNAAPA> ObterPorUeIdMesAnoLetivoProfissional(long ueId, int mes, int anoLetivo, string rfProfissional);
        Task<IEnumerable<ConsolidadoAtendimentoNAAPA>> ObterPorUeIdAnoLetivo(long ueId, int anoLetivo);
        Task<IEnumerable<QuantidadeEncaminhamentoNAAPAEmAbertoDto>> ObterQuantidadeEncaminhamentoNAAPAEmAberto(int anoLetivo, string codigoDre);
    }
}