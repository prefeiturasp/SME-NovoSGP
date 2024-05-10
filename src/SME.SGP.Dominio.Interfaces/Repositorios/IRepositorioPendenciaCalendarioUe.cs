using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaCalendarioUe : IRepositorioBase<PendenciaCalendarioUe>
    {
        Task<IEnumerable<PendenciaCalendarioUe>> ObterPendenciasPorCalendarioUe(long tipoCalendarioId, long ueId, TipoPendencia tipoPendencia);
        Task<IEnumerable<long>> ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomatico(long idUe,int anoLetivo);
    }
}
