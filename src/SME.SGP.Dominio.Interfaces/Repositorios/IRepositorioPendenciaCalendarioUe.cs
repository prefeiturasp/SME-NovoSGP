using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaCalendarioUe : IRepositorioBase<PendenciaCalendarioUe>
    {
        Task<PendenciaCalendarioUe> ObterPendenciaPorCalendarioUe(long tipoCalendarioId, long ueId, TipoPendencia tipoPendencia);
    }
}
