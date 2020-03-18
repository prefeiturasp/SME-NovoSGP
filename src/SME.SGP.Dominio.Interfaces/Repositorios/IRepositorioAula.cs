using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAula : IRepositorioBase<Aula>
    {
        Task<AulaConsultaDto> ObterAulaDataTurmaDisciplina(DateTime data, string turmaId, string disciplinaId);
        Task<AulaConsultaDto> ObterAulaDataTurmaDisciplinaProfessorRf(DateTime data, string turmaId, string disciplinaId, string professorRf);
        Task<IEnumerable<DateTime>> ObterDatasAulasExistentes(List<DateTime> datas, string turmaId, string disciplinaId, string professorRf);
        Task<AulaConsultaDto> ObterAulaIntervaloTurmaDisciplina(DateTime dataInicio, DateTime dataFim, string turmaId, long atividadeAvaliativaId);

        Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string CodigoRf, int? mes = null, int? semanaAno = null, string disciplinaId = null);

        Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string CodigoRf);

        Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, int mes, string CodigoRf);

        Task<IEnumerable<AulaDto>> ObterAulas(string turmaId, string ueId, string codigoRf, DateTime? data, string disciplinaId);

        Task<IEnumerable<AulaDto>> ObterAulas(string turmaId, string ueId, string codigoRf, DateTime? data, string[] disciplinasId);

        Task<IEnumerable<AulaCompletaDto>> ObterAulasCompleto(long tipoCalendarioId, string turmaId, string ueId, DateTime data, Guid perfil, bool turmaHistorica = false);

        Task<IEnumerable<Aula>> ObterAulasRecorrencia(long aulaPaiId, long? aulaIdInicioRecorrencia = null, DateTime? dataFinal = null);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaDiaProfessor(string turma, string disciplina, DateTime dataAula, string codigoRf);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaSemanaProfessor(string turma, string disciplina, int semana, string codigoRf);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaExperienciasPedagogicasDia(string turma, DateTime dataAula);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaExperienciasPedagogicasSemana(string turma, int semana);

        Aula ObterCompletoPorId(long id);

        IEnumerable<AulaConsultaDto> ObterDatasDeAulasPorAnoTurmaEDisciplina(int anoLetivo, string turmaId, string disciplinaId, long usuarioId, string usuarioRF, bool aulaCJ, bool ehDiretorOuSupervisor);

        Aula ObterPorWorkflowId(long workflowId);

        bool UsuarioPodeCriarAulaNaUeTurmaEModalidade(Aula aula, ModalidadeTipoCalendario modalidade);
    }
}