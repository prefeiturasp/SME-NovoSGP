using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaPlanoAEE : IRepositorioBase<PendenciaPlanoAEE>
    {
        Task<IEnumerable<PendenciaPlanoAEE>> ObterPorPlanoId(long planoAEEId);
        Task<bool> ExistePendenciaPorPlano(long planoAeeId);
    }
}
