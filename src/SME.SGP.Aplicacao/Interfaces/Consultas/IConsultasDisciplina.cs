using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasDisciplina
    {
        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasParaPlanejamento(FiltroDisciplinaPlanejamentoDto filtroDisciplinaPlanejamentoDto);

        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorProfessorETurma(long codigoTurma);
    }
}