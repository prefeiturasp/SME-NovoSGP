using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAula : IRepositorioBase<Aula>
    {
        Task<bool> ExisteAulaNaDataAsync(DateTime data, string turmaCodigo, string componenteCurricular);

        Task<bool> ExisteAulaNaDataDataTurmaDisciplinaProfessorRfAsync(DateTime data, string turmaId, string disciplinaId, string professorRf);

        Task<AulaConsultaDto> ObterAulaDataTurmaDisciplina(DateTime data, string turmaId, string disciplinaId);

        Task<AulaConsultaDto> ObterAulaDataTurmaDisciplinaProfessorRf(DateTime data, string turmaId, string disciplinaId, string professorRf);

        Task<AulaConsultaDto> ObterAulaIntervaloTurmaDisciplina(DateTime dataInicio, DateTime dataFim, string turmaId, long atividadeAvaliativaId);

        Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string codigoRf, int? mes = null, int? semanaAno = null, string disciplinaId = null);

        Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string codigoRf);

        Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string codigoRf, int? mes = null);

        Task<IEnumerable<AulaDto>> ObterAulas(string turmaId, string ueId, string codigoRf, DateTime? data, string disciplinaId);

        Task<IEnumerable<AulaDto>> ObterAulas(string turmaId, string ueId, string codigoRf, DateTime? data, string[] disciplinasId);

        Task<IEnumerable<AulaCompletaDto>> ObterAulasCompleto(long tipoCalendarioId, string turmaId, string ueId, DateTime data, Guid perfil);

        Task<IEnumerable<AulaConsultaDto>> ObterAulasPorDataTurmaComponenteCurricularProfessorRf(DateTime data, string turmaId, string disciplinaId, string professorRf);

        Task<IEnumerable<Aula>> ObterAulasProfessorCalendarioPorData(long tipoCalendarioId, string turmaCodigo, string ueCodigo, DateTime dataDaAula);

        Task<IEnumerable<Aula>> ObterAulasProfessorCalendarioPorMes(long tipoCalendarioId, string turmaCodigo, string ueCodigo, int mes);

        Task<IEnumerable<Aula>> ObterAulasRecorrencia(long aulaPaiId, long? aulaIdInicioRecorrencia = null, DateTime? dataFinal = null);

        IEnumerable<Aula> ObterAulasReposicaoPendentes(string codigoTurma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        IEnumerable<Aula> ObterAulasSemFrequenciaRegistrada(string codigoTurma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        IEnumerable<Aula> ObterAulasSemPlanoAulaNaDataAtual(string codigoTurma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaDiaProfessor(string turma, string disciplina, DateTime dataAula, string codigoRf);

        Task<int> ObterQuantidadeAulasTurmaComponenteCurricularDiaProfessor(string turma, string componenteCurricular, DateTime dataAula, string codigoRf);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaSemanaProfessor(string turma, string componenteCurricular, int semana, string codigoRf);

        Task<int> ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(string turma, string disciplina, int semana, string codigoRf, DateTime dataExcecao);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaExperienciasPedagogicasDia(string turma, DateTime dataAula);

        Task<int> ObterQuantidadeAulasTurmaExperienciasPedagogicasDia(string turma, DateTime dataAula);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaExperienciasPedagogicasSemana(string turma, int semana);

        Task<int> ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(string turma, int semana);

        Task<Aula> ObterCompletoPorIdAsync(long id);

        Task<DateTime> ObterDataAula(long aulaId);

        Task<IEnumerable<DateTime>> ObterDatasAulasExistentes(List<DateTime> datas, string turmaId, string disciplinaId, string professorRf, long? aulaPaiId = null);

        IEnumerable<AulaConsultaDto> ObterDatasDeAulasPorAnoTurmaEDisciplina(long periodoEscolarId, int anoLetivo, string turmaCodigo, string disciplinaId, string usuarioRF, bool aulaCJ = false, bool ehDiretorOuSupervisor = false);

        Aula ObterPorWorkflowId(long workflowId);

        Task<int> ObterQuantidadeDeAulasPorTurmaDisciplinaPeriodoAsync(string turmaId, string disciplinaId, DateTime inicio, DateTime fim);

        IEnumerable<DateTime> ObterUltimosDiasLetivos(DateTime dataReferencia, int quantidadeDias, long tipoCalendarioId);

        bool UsuarioPodeCriarAulaNaUeTurmaEModalidade(Aula aula, ModalidadeTipoCalendario modalidade);
    }
}