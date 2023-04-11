using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasDisciplina
    {
        Task<IEnumerable<DisciplinaResposta>> ObterComponentesCJ(Modalidade? modalidade, string codigoTurma, string ueId, long codigoDisciplina, string rf, bool ignorarDeParaRegencia = false);

        Task<List<DisciplinaDto>> ObterComponentesCurricularesPorProfessorETurma(string codigoTurma, bool turmaPrograma, bool realizarAgrupamentoComponente = false);

        Task<IEnumerable<DisciplinaDto>> ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(long codigoDisciplina, string codigoTurma, bool turmaPrograma, bool temRegencia);

        Task<List<DisciplinaDto>> ObterDisciplinasAgrupadasPorProfessorETurma(string codigoTurma, bool turmaPrograma);

        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasCJ(string codigoTurma, string login, bool verificaPerfilCP = false, string codigoDre = null, string codigoUe = null);

        Task<List<DisciplinaDto>> ObterDisciplinasPorProfessorETurma(string codigoTurma, bool turmaPrograma);

        Task<List<DisciplinaDto>> ObterDisciplinasPorTurma(string codigoTurma, bool turmaPrograma);

        Task<DisciplinaDto> ObterDisciplina(long disciplinaId);

        Task<IEnumerable<DisciplinaResposta>> ObterComponentesRegencia(Turma turma);
        IEnumerable<DisciplinaDto> MapearParaDto(IEnumerable<DisciplinaResposta> disciplinas);
        IEnumerable<DisciplinaResposta> MapearComponentes(IEnumerable<ComponenteCurricularEol> componentesCurriculares);
    }
}