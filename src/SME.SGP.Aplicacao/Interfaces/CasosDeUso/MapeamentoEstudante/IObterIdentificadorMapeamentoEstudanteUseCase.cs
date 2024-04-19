using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterIdentificadorMapeamentoEstudanteUseCase
    {
        Task<long?> Executar(string codigoAluno, long turmaId, int bimestre);
    }
}
