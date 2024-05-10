using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterAlunosSinalizadosPrioridadeMapeamentoEstudanteUseCase
    {
        Task<AlunoSinalizadoPrioridadeMapeamentoEstudanteDto[]> Executar(long turmaId, int bimestre);
    }
}
