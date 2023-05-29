using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidadoEncaminhamentoNAAPA  : IRepositorioBase<ConsolidadoEncaminhamentoNAAPA>
    {
        Task<IEnumerable<ConsolidadoEncaminhamentoNAAPA>> ObterPorUeIdAnoLetivo(long ueId,int anoLetivo);
        Task<ConsolidadoEncaminhamentoNAAPA> ObterPorUeIdAnoLetivoSituacao(long ueId,int anoLetivo, int situacao);
        Task<IEnumerable<DadosGraficoSitaucaoPorUeAnoLetivoDto>> ObterDadosGraficoSitaucaoPorUeAnoLetivo(int anoLetivo, long? ueId);
    }
}