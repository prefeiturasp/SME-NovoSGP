using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAula
    {
        AulaConsultaDto BuscarPorId(long id);

        Task<IEnumerable<DataAulasProfessorDto>> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(int anoLetivo, string turma, string disciplina);

        Task<int> ObterQuantidadeAulasTurmaSemana(string turma, string disciplina, string semana);

        Task<AulaConsultaDto> ObterAulaDataTurmaDisciplina(DateTime data, string turmaId, string disciplinaId);
    }
}