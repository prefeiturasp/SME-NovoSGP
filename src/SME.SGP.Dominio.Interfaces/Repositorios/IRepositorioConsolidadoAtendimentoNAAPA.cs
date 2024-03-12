using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidadoAtendimentoNAAPA : IRepositorioBase<ConsolidadoAtendimentoNAAPA>
    {
        Task<ConsolidadoAtendimentoNAAPA> ObterPorUeIdMesAnoLetivoProfissional(long ueId, int mes, int anoLetivo, string rfProfissional, int modadalidade);
        Task<IEnumerable<long>> ObterIds(long ueId, int mes, int anoLetivo, string[] rfsProfissionaisIgnorados);
        Task<IEnumerable<GraficoQuantitativoNAAPADto>> ObterQuantidadeAtendimentoNAAPAPorProfissionalMes(int anoLetivo, long dreId, long? ueId, int? mes, int? modalidade);
    }
}