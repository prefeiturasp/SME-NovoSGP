using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{

    public interface IObterParecerConclusivoAlunoTurmaUseCase
    {
        Task<ParecerConclusivoDto> Executar(string codigoTurma, string alunoCodigo);
    }
}