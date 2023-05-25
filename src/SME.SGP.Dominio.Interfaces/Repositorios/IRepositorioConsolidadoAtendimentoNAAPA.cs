using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidadoAtendimentoNAAPA : IRepositorioBase<ConsolidadoAtendimentoNAAPA>
    {
        Task<IEnumerable<ConsolidadoAtendimentoNAAPA>> ObterPorUeIdAnoLetivo(long ueId, int anoLetivo);
    }
}