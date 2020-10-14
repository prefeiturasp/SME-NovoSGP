using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPlanejamentoAnualPorTurmaComponenteUseCase
    {
        Task<long> Executar(long turmaId, long componenteCurricularId);
    }
}