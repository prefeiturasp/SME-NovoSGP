using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidadoAtendimentoNAAPA : IRepositorioBase<ConsolidadoAtendimentoNAAPA>
    {
        Task<ConsolidadoAtendimentoNAAPA> ObterPorUeIdMesAnoLetivoProfissional(long ueId, int mes, int anoLetivo, string profissional);
    }
}