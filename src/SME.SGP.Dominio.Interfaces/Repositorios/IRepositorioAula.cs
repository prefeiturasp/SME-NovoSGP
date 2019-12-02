using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAula : IRepositorioBase<Aula>
    {
        Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string rf, int? mes = null, int? semanaAno = null);
        Task<IEnumerable<Aula>> ObterAulasRecorrencia(long aulaPaiId, long? aulaIdInicioRecorrencia = null, DateTime? dataFinal = null);
        Task<AulaConsultaDto> ObterAulaDataTurmaDisciplina(DateTime data, string turmaId, string disciplinaId);

        Task<IEnumerable<AulaCompletaDto>> ObterAulasCompleto(long tipoCalendarioId, string turmaId, string ueId, DateTime data, Guid perfil, string rf);

        IEnumerable<Aula> ObterAulasPorTurmaEAnoLetivo(string turmaId, string anoLetivo);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaSemana(string turma, string disciplina, string semana);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaExperienciasPedagogicasSemana(string turma, string semana);

        IEnumerable<AulaConsultaDto> ObterDatasDeAulasPorAnoTurmaEDisciplina(int anoLetivo, string turmaId, string disciplinaId, long usuarioId, string usuarioRF, Guid perfil);

        Aula ObterPorWorkflowId(long workflowId);

        bool UsuarioPodeCriarAulaNaUeTurmaEModalidade(Aula aula, ModalidadeTipoCalendario modalidade);
    }
}