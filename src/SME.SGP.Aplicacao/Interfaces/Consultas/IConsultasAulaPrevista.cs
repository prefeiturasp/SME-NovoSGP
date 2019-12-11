using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAulaPrevista
    {
        Task<AulasPrevistasDadasAuditoriaDto> BuscarPorId(long id);
    }
}
