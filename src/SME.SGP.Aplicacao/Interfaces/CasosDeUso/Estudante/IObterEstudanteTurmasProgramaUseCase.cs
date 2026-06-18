using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterEstudanteTurmasProgramaUseCase
    {
        Task<IEnumerable<AlunoTurmaProgramaDto>> Executar(string codigoAluno, int? anoLetivo, bool filtrarSituacaoMatricula);
    }
}

