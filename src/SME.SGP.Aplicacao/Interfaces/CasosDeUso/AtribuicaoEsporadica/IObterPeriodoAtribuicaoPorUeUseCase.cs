using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPeriodoAtribuicaoPorUeUseCase
    {
        Task<PeriodoAtribuicaoEsporadicaDto> Executar(long ueId, int anoLetivo);
    }
}