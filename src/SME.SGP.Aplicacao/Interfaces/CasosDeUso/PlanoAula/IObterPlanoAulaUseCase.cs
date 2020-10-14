using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPlanoAulaUseCase
    {
        Task<PlanoAulaRetornoDto> Executar(long aulaId);
    }
}