using SME.SGP.Dominio;
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

        Task<int> ObterQuantidadeAulasTurmaSemanaProfessor(string turma, string disciplina, string semana, string codigoRf);

        Task<int> ObterQuantidadeAulasRecorrentes(long aulaInicialId, RecorrenciaAula recorrencia);

        Task<int> ObterRecorrenciaDaSerie(long aulaId);

        Task<AulaConsultaDto> ObterAulaDataTurmaDisciplina(DateTime data, string turmaId, string disciplinaId);

        Task<IEnumerable<AulasPrevistasDadasDto>> ObterAulaPrevistaDada(string turmaId, string disciplinaId);

    }
}