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

        Task<int> ObterQuantidadeAulasTurmaDiaProfessor(string turma, string disciplina, DateTime dataAula, string codigoRf);

        Task<int> ObterQuantidadeAulasRecorrentes(long aulaInicialId, RecorrenciaAula recorrencia);

        Task<int> ObterRecorrenciaDaSerie(long aulaId);

        Task<bool> ChecarFrequenciaPlanoNaRecorrencia(long aulaId);

        Task<bool> ChecarFrequenciaPlanoAula(long aulaId);
    }
}