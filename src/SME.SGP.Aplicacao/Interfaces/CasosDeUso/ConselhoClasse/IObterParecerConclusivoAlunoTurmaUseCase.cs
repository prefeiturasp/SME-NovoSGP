using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{

    public interface IObterParecerConclusivoAlunoTurmaUseCase
    {
        Task<ParecerConclusivoDto> Executar(string codigoTurma, string alunoCodigo);
    }
}