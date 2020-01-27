using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasDisciplina
    {
        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasParaPlanejamento(FiltroDisciplinaPlanejamentoDto filtroDisciplinaPlanejamentoDto);

        Task<List<DisciplinaDto>> ObterDisciplinasPorProfessorETurma(string codigoTurma, bool turmaPrograma);

        Task<List<DisciplinaDto>> ObterDisciplinasPorTurma(string codigoTurma, bool turmaPrograma);
    }
}