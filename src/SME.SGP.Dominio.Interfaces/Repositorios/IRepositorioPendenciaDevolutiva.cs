using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaDevolutiva
    {
        Task<IEnumerable<PendenciaDevolutiva>> ObterPendenciasDevolutivaPorTurmaComponente(long turmaId, long componenteId);
        Task<IEnumerable<PendenciaDevolutiva>> ObterPendenciasDevolutivaPorPendencia(long pendenciaId);
    }
}
