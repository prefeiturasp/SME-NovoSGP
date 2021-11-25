using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoEol
    {
        Task AlterarEmail(string login, string email);

        Task<AlterarSenhaRespostaDto> AlterarSenha(string login, string novaSenha);

        Task AtribuirCJSeNecessario(Guid usuarioId);

        Task AtribuirCJSeNecessario(string codigoRf);

        Task AtribuirPerfil(string codigoRf, Guid perfil);

        Task<UsuarioEolAutenticacaoRetornoDto> Autenticar(string login, string senha);

        IEnumerable<CicloRetornoDto> BuscarCiclos();

        IEnumerable<TipoEscolaRetornoDto> BuscarTiposEscola();

        Task<bool> ExisteUsuarioComMesmoEmail(string login, string email);

        Task<AbrangenciaRetornoEolDto> ObterAbrangencia(string login, Guid perfil);

        Task<AbrangenciaCompactaVigenteRetornoEOLDTO> ObterAbrangenciaCompactaVigente(string login, Guid perfil);

        Task<AbrangenciaRetornoEolDto> ObterAbrangenciaParaSupervisor(string[] uesIds);        
        Task<string[]> ObterAdministradoresSGP(string codigoDreOuUe);

        Task<string[]> ObterAdministradoresSGPParaNotificar(string codigoDreOuUe);

        Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosAtivosPorTurma(long turmaId);

        Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string turmaId);

        [Obsolete("não utilizar mais esse método, utilize o ObterAlunosPorTurma")]
        Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string turmaId, int anoLetivo);

        Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorNomeCodigoEol(string anoLetivo, string codigoUe, long codigoTurma, string nome, long? codigoEol, bool? somenteAtivos);

        Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil);

        Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamento(string codigoTurma, string login, Guid perfil);

        Task<IEnumerable<AlunoPorTurmaResposta>> ObterDadosAluno(string codidoAluno, int anoLetivo);

        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasParaPlanejamento(long codigoTurma, string login, Guid perfil);

        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurma(string codigoTurma);

        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil);

        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIdsSemAgrupamento(long[] ids);

        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIdsAgrupadas(long[] ids);

        IEnumerable<DreRespostaEolDto> ObterDres();

        IEnumerable<EscolasRetornoDto> ObterEscolasPorCodigo(string[] codigoUes);

        IEnumerable<EscolasRetornoDto> ObterEscolasPorDre(string dreId);

        EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorDre();

        EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorTurma(string[] codigosTurma = null);

        IEnumerable<UsuarioEolRetornoDto> ObterFuncionariosPorCargoUe(string ueId, long cargoId);

        Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorUe(BuscaFuncionariosFiltroDto buscaFuncionariosFiltroDto);

        Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorDre(Guid perfil, FiltroFuncionarioDto filtroFuncionariosDto);

        Task<IEnumerable<ProfessorResumoDto>> ObterListaNomePorListaRF(IEnumerable<string> codigosRF);

        Task<IEnumerable<ProfessorResumoDto>> ObterListaResumosPorListaRF(IEnumerable<string> codigosRF, int anoLetivo);

        IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf);

        Task<MeusDadosDto> ObterMeusDados(string login);

        Task<UsuarioEolAutenticacaoRetornoDto> ObterPerfisPorLogin(string login);

        Task<int[]> ObterPermissoesPorPerfil(Guid perfilGuid);

        Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string ueId,string nomeProfessor);

        Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string nomeProfessor, bool incluirEmei);

        Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesDisciplinas(string turmaCodigo, string professorRf = null);
        Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesPorTurmas(IEnumerable<string> codigosTurmas);

        Task<string> ObterNomeProfessorPeloRF(string rfProfessor);
        Task<UsuarioResumoCoreDto> ObterResumoCore(string login);

        Task<ProfessorResumoDto> ObterResumoProfessorPorRFAnoLetivo(string codigoRF, int anoLetivo, bool buscarOutrosCargos = false);
        Task<ProfessorResumoDto> ObterProfessorPorRFUeDreAnoLetivo(string codigoRF, int anoLetivo, string dreId,string ueId);

        IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorCodigo(string[] codigoSupervisores);

        IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorDre(string dreId);

        Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo);

        Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopiaPlanoAnual(string codigoRf, long componenteCurricularId, int codigoTurma);

        Task<IEnumerable<TurmaPorUEResposta>> ObterTurmasPorUE(string ueId, string anoLetivo);

        Task<bool> PodePersistirTurma(string professorRf, string codigoTurma, DateTime data);

        Task<bool> PodePersistirTurmaDisciplina(string professorRf, string codigoTurma, string disciplinaId, DateTime data);

        Task<bool> PodePersistirTurmaNoPeriodo(string professorRf, string codigoTurma, long componenteCurricularId, DateTime dataInicio, DateTime dataFim);       

        Task<bool> ProfessorPodePersistirTurma(string professorRf, string codigoTurma, DateTime data);

        Task ReiniciarSenha(string login);
        Task<UsuarioEolAutenticacaoRetornoDto> RelecionarUsuarioPerfis(string login);
        Task RemoverCJSeNecessario(Guid usuarioId);
        Task<bool> ValidarProfessor(string professorRf);
        Task<bool> TurmaPossuiComponenteCurricularPAP(string codigoTurma, string login, Guid idPerfil);
        Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorAnosEModalidade(string codigoUe, Modalidade modalidade, string[] anosEscolares, int anoLetivo);
        Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesTurmasProgramaPorAnoEModalidade(string codigoUe, Modalidade modalidade, int anoLetivo);
        Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesCurriculares();
        Task<InformacoesEscolaresAlunoDto> ObterNecessidadesEspeciaisAluno(string codigoAluno);       

        Task<IEnumerable<string>> DefinirTurmasRegulares(string[] codigosTurmas);
        Task<DadosTurmaEolDto> ObterDadosTurmaPorCodigo(string codigoTurma);
        Task<AtribuicaoProfessorTurmaEOLDto> VerificaAtribuicaoProfessorTurma(string professorRf, string codigoTurma);

        Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorCargoUeAsync(string ueId, long cargoId);

        Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesRegenciaPorAno(int anoTurma);
        Task<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>> ListagemTurmasComComponente(string codigoUe, string modalidade, int semestre, string codigoTurma, int anoLetivo, int qtdeRegistos, int qtdeRegistrosIgnorados);
    }
}