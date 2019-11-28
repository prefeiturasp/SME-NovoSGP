using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAula : IRepositorioBase<Aula>
    {
        Task<AulaConsultaDto> ObterAulaDataTurmaDisciplina(DateTime data, string turmaId, string disciplinaId);

        Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string rf);

        Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, int mes, string rf);

        Task<IEnumerable<AulaCompletaDto>> ObterAulasCompleto(long tipoCalendarioId, string turmaId, string ueId, DateTime data, Guid perfil, string rf);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaSemana(string turma, string disciplina, string semana);

        Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaExperienciasPedagogicasSemana(string turma, string semana);

        IEnumerable<AulaConsultaDto> ObterDatasDeAulasPorAnoTurmaEDisciplina(int anoLetivo, string turmaId, string disciplinaId, long usuarioId, string usuarioRF, Guid perfil);

        bool UsuarioPodeCriarAulaNaUeTurmaEModalidade(Aula aula, ModalidadeTipoCalendario modalidade);
    }
}