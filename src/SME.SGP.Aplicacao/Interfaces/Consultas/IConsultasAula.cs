using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAula
    {
        AulaConsultaDto BuscarPorId(long id);

        Task<IEnumerable<DataAulasProfessorDto>> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(long calendarioId, string turma, string disciplina);

        Task<int> ObterQuantidadeAulasTurma(string turma, string disciplina);
    }
}