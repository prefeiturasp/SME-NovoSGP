using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasDisciplina
    {
        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasParaPlanejamento(long codigoTurma);

        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorProfessorETurma(long codigoTurma, string rfProfessor);
    }
}