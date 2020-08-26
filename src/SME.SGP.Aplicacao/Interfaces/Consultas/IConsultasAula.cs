using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAula
    {
        Task<AulaConsultaDto> BuscarPorId(long id);

        Task<bool> AulaDentroPeriodo(Aula aula);

        Task<bool> AulaDentroPeriodo(string turmaId, DateTime dataAula);

        Task<IEnumerable<DataAulasProfessorDto>> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(int anoLetivo, string turmaCodigo, string disciplinaCodigo);

        Task<int> ObterQuantidadeAulasRecorrentes(long aulaInicialId, RecorrenciaAula recorrencia);

        Task<int> ObterQuantidadeAulasTurmaDiaProfessor(string turma, string disciplina, DateTime dataAula, string codigoRf);

        Task<int> ObterQuantidadeAulasTurmaSemanaProfessor(string turma, string disciplina, int semana, string codigoRf);

        int ObterRecorrenciaDaSerie(long aulaId);
    }
}