using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaPlanoAEE : IRepositorioBase<PendenciaPlanoAEE>
    {
        Task<IEnumerable<PendenciaPlanoAEE>> ObterPorPlanoId(long planoAEEId);
    }
}
