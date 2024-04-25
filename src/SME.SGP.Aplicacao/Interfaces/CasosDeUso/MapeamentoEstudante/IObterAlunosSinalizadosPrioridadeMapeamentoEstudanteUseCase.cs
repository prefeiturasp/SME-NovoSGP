using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterAlunosSinalizadosPrioridadeMapeamentoEstudanteUseCase
    {
        Task<string[]> Executar(long turmaId);
    }
}
