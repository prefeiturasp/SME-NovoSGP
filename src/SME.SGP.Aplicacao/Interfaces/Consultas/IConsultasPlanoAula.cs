using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPlanoAula
    {
        Task<IEnumerable<PlanoAulaExistenteRetornoDto>> ValidarPlanoAulaExistente(FiltroPlanoAulaExistenteDto filtroPlanoAulaExistenteDto);
        Task<bool> PlanoAulaRegistrado(long aulaId);
    }
}
