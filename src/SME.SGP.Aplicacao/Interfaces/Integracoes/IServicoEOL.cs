using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoEOL
    {
        Task AlterarEmail(string login, string email);

        Task<AlterarSenhaRespostaDto> AlterarSenha(string login, string novaSenha);

        Task AtribuirCJSeNecessario(Guid usuarioId);

        Task AtribuirCJSeNecessario(string codigoRf);

        Task<UsuarioEolAutenticacaoRetornoDto> Autenticar(string login, string senha);

        Task<bool> ExisteUsuarioComMesmoEmail(string login, string email);

        Task<AbrangenciaRetornoEolDto> ObterAbrangencia(string login, Guid perfil);

        Task<AbrangenciaCompactaVigenteRetornoEOLDTO> ObterAbrangenciaCompactaVigente(string login, Guid perfil);

        Task<AbrangenciaRetornoEolDto> ObterAbrangenciaParaSupervisor(string[] uesIds);

        Task<string[]> ObterAdministradoresSGP(string codigoDreOuUe);

        Task<string[]> ObterAdministradoresSGPParaNotificar(string codigoDreOuUe);

        Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosAtivosPorTurma(long turmaId);

        Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string turmaId);

        Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string turmaId, int anoLetivo);

        Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil);

        Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamento(string codigoTurma, string login, Guid perfil);

        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasParaPlanejamento(long codigoTurma, string login, Guid perfil);

        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurma(string codigoTurma);

        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil);

        IEnumerable<DisciplinaDto> ObterDisciplinasPorIds(long[] ids);

        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIdsAsync(long[] ids);

        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIdsSemAgrupamento(long[] ids);

        IEnumerable<DreRespostaEolDto> ObterDres();

        IEnumerable<EscolasRetornoDto> ObterEscolasPorCodigo(string[] codigoUes);

        IEnumerable<EscolasRetornoDto> ObterEscolasPorDre(string dreId);

        EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorDre();

        EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorTurma(string[] codigosTurma = null);

        IEnumerable<UsuarioEolRetornoDto> ObterFuncionariosPorCargoUe(string UeId, long cargoId);

        Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorUe(BuscaFuncionariosFiltroDto buscaFuncionariosFiltroDto);

        Task<IEnumerable<ProfessorResumoDto>> ObterListaNomePorListaRF(IEnumerable<string> codigosRF);

        Task<IEnumerable<ProfessorResumoDto>> ObterListaResumosPorListaRF(IEnumerable<string> codigosRF, int anoLetivo);

        IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf);

        Task<MeusDadosDto> ObterMeusDados(string login);

        Task<UsuarioEolAutenticacaoRetornoDto> ObterPerfisPorLogin(string login);

        Task<int[]> ObterPermissoesPorPerfil(Guid perfilGuid);

        Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string nomeProfessor);

        Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string nomeProfessor, bool incluirEmei);

        Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesDisciplinas(string turmaCodigo, string professorRf = null);

        Task<UsuarioResumoCoreDto> ObterResumoCore(string login);

        Task<ProfessorResumoDto> ObterResumoProfessorPorRFAnoLetivo(string codigoRF, int anoLetivo);

        Task<ProfessorResumoDto> ObterResumoProfessorPorRFAnoLetivo(string codigoRF, int anoLetivo, bool incluirEmei);

        IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorCodigo(string[] codigoSupervisores);

        IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorDre(string dreId);

        Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo);

        Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopiaPlanoAnual(string codigoRf, long componenteCurricularId, int codigoTurma);

        Task<IEnumerable<TurmaPorUEResposta>> ObterTurmasPorUE(string ueId, string anoLetivo);

        Task<bool> PodePersistirTurma(string professorRf, string codigoTurma, DateTime data);

        Task<bool> PodePersistirTurmaDisciplina(string professorRf, string codigoTurma, string disciplinaId, DateTime data);

        Task<IEnumerable<PodePersistirNaDataRetornoEolDto>> PodePersistirTurmaNasDatas(string professorRf, string codigoTurma, string[] datas, long codigoDisciplina);

        Task<bool> ProfessorPodePersistirTurma(string professorRf, string codigoTurma, DateTime data);

        Task ReiniciarSenha(string login);

        Task<UsuarioEolAutenticacaoRetornoDto> RelecionarUsuarioPerfis(string login);

        Task RemoverCJSeNecessario(Guid usuarioId);

        Task<bool> ValidarProfessor(string professorRf);
    }
}