using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAEEReestruturacao : IRepositorioBase<PlanoAEEReestruturacao>
    {
        Task<IEnumerable<PlanoAEEReestruturacaoDto>> ObterRestruturacoesPorPlanoAEEId(long planoId);
    }
}
