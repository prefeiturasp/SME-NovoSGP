using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterEstudanteTurmasProgramaUseCase 
    {
        Task<IEnumerable<AlunoTurmaProgramaDto>> Executar(string codigoAluno, int? anoLetivo, bool filtrarSituacaoMatricula);
    }
}

