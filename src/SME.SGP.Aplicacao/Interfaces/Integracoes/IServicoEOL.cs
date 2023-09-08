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

        Task<AutenticacaoApiEolDto> Autenticar(string login, string senha);

        IEnumerable<CicloRetornoDto> BuscarCiclos();

        IEnumerable<TipoEscolaRetornoDto> BuscarTiposEscola();

        Task<bool> ExisteUsuarioComMesmoEmail(string login, string email);

        Task<AbrangenciaRetornoEolDto> ObterAbrangencia(string login, Guid perfil);

        Task<AbrangenciaCompactaVigenteRetornoEOLDTO> ObterAbrangenciaCompactaVigente(string login, Guid perfil);

        Task<AbrangenciaRetornoEolDto> ObterAbrangenciaParaSupervisor(string[] uesIds);
        Task<string[]> ObterAdministradoresSGP(string codigoDreOuUe);

        Task<string[]> ObterAdministradoresSGPParaNotificar(string codigoDreOuUe);

        Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorNomeCodigoEol(string anoLetivo, string codigoUe, long codigoTurma, string nome, long? codigoEol, bool? somenteAtivos);

        Task<IEnumerable<AlunoPorTurmaResposta>> ObterDadosAluno(string codidoAluno, int anoLetivo, bool consideraHistorico, bool filtrarSituacao = true, bool verificarTipoTurma = true);
        IEnumerable<DreRespostaEolDto> ObterDres();

        IEnumerable<EscolasRetornoDto> ObterEscolasPorCodigo(string[] codigoUes);

        IEnumerable<EscolasRetornoDto> ObterEscolasPorDre(string dreId);

        EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorDre();

        EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorTurma(string[] codigosTurma = null);
        
        Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorUe(BuscaFuncionariosFiltroDto buscaFuncionariosFiltroDto);

        Task<IEnumerable<ProfessorResumoDto>> ObterListaNomePorListaRF(IEnumerable<string> codigosRF);
        Task<IEnumerable<FuncionarioUnidadeDto>> ObterListaNomePorListaLogin(IEnumerable<string> logins);

        Task<IEnumerable<ProfessorResumoDto>> ObterListaResumosPorListaRF(IEnumerable<string> codigosRF, int anoLetivo);

        IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf);

        Task<MeusDadosDto> ObterMeusDados(string login);

        Task<PerfisApiEolDto> ObterPerfisPorLogin(string login);

        Task<RetornoDadosAcessoUsuarioSgpDto> CarregarDadosAcessoPorLoginPerfil(string login, Guid perfilGuid, AdministradorSuporteDto administradorSuporte = null);

        Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string ueId, string nomeProfessor);

        Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string nomeProfessor, bool incluirEmei);
        
        Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesPorTurmas(IEnumerable<string> codigosTurmas);
        Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesPorUe(string ueCodigo, DateTime dataReferencia);

        Task<string> ObterNomeProfessorPeloRF(string rfProfessor);
        Task<UsuarioResumoCoreDto> ObterResumoCore(string login);

        Task<ProfessorResumoDto> ObterResumoProfessorPorRFAnoLetivo(string codigoRF, int anoLetivo, bool buscarOutrosCargos = false);
        Task<ProfessorResumoDto> ObterProfessorPorRFUeDreAnoLetivo(string codigoRF, int anoLetivo, string dreId, string ueId, bool buscarOutrosCargos = false, bool buscarPorTodasDre = false);

        IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorCodigo(string[] codigoSupervisores);

        Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo);

        Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopiaPlanoAnual(string codigoRf, long componenteCurricularId, int codigoTurma);

        Task<bool> PodePersistirTurma(string professorRf, string codigoTurma, DateTime data);

        Task<bool> PodePersistirTurmaDisciplina(string professorRf, string codigoTurma, string disciplinaId, DateTime data);

        Task<bool> PodePersistirTurmaNoPeriodo(string professorRf, string codigoTurma, long componenteCurricularId, DateTime dataInicio, DateTime dataFim);

        Task ReiniciarSenha(string login);
        Task<UsuarioEolAutenticacaoRetornoDto> RelecionarUsuarioPerfis(string login);
        Task RemoverCJSeNecessario(Guid usuarioId);
        Task<bool> ValidarProfessor(string professorRf);
        Task<bool> TurmaPossuiComponenteCurricularPAP(string codigoTurma, string login, Guid idPerfil);
        Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorAnosEModalidade(string codigoUe, Modalidade modalidade, int anoLetivo, string[] anosEscolares);
        Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesCurriculares();
        Task<InformacoesEscolaresAlunoDto> ObterNecessidadesEspeciaisAluno(string codigoAluno);

        Task<IEnumerable<string>> DefinirTurmasRegulares(string[] codigosTurmas);
        Task<AtribuicaoProfessorTurmaEOLDto> VerificaAtribuicaoProfessorTurma(string professorRf, string codigoTurma);

        Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorCargoUeAsync(string ueId, long cargoId);
        Task<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>> ListagemTurmasComComponente(string codigoUe, string modalidade, int semestre, string codigoTurma, int anoLetivo, int qtdeRegistos, int qtdeRegistrosIgnorados);

        Task<IEnumerable<UsuarioEolRetornoDto>> ObterUsuarioFuncionario(Guid perfil, FiltroFuncionarioDto filtroFuncionariosDto);

        Task<AutenticacaoApiEolDto> ObtenhaAutenticacaoSemSenha(string login);
    }
}