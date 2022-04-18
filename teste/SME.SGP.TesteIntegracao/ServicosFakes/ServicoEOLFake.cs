using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ServicoEOLFake : IServicoEol
    {
        public Task AlterarEmail(string login, string email)
        {
            throw new NotImplementedException();
        }

        public Task<AlterarSenhaRespostaDto> AlterarSenha(string login, string novaSenha)
        {
            throw new NotImplementedException();
        }

        public Task AtribuirCJSeNecessario(Guid usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task AtribuirCJSeNecessario(string codigoRf)
        {
            throw new NotImplementedException();
        }

        public Task AtribuirPerfil(string codigoRf, Guid perfil)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioEolAutenticacaoRetornoDto> Autenticar(string login, string senha)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CicloRetornoDto> BuscarCiclos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoEscolaRetornoDto> BuscarTiposEscola()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> DefinirTurmasRegulares(string[] codigosTurmas)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExisteUsuarioComMesmoEmail(string login, string email)
        {
            throw new NotImplementedException();
        }

        public Task<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>> ListagemTurmasComComponente(string codigoUe, string modalidade, int semestre, string codigoTurma, int anoLetivo, int qtdeRegistos, int qtdeRegistrosIgnorados)
        {
            throw new NotImplementedException();
        }

        public Task<AbrangenciaRetornoEolDto> ObterAbrangencia(string login, Guid perfil)
        {
            throw new NotImplementedException();
        }

        public Task<AbrangenciaCompactaVigenteRetornoEOLDTO> ObterAbrangenciaCompactaVigente(string login, Guid perfil)
        {
            throw new NotImplementedException();
        }

        public Task<AbrangenciaRetornoEolDto> ObterAbrangenciaParaSupervisor(string[] uesIds)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ObterAdministradoresSGP(string codigoDreOuUe)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ObterAdministradoresSGPParaNotificar(string codigoDreOuUe)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosAtivosPorTurma(string codigoTurma, DateTime dataAula)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorNomeCodigoEol(string anoLetivo, string codigoUe, long codigoTurma, string nome, long? codigoEol, bool? somenteAtivos)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string turmaId, bool consideraInativos = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesCurriculares()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorAnosEModalidade(string codigoUe, Modalidade modalidade, string[] anosEscolares, int anoLetivo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil, bool realizarAgrupamentoComponente = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamento(string codigoTurma, string login, Guid perfil)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorLoginEIdPerfil(string login, Guid idPerfil)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesTurmasProgramaPorAnoEModalidade(string codigoUe, Modalidade modalidade, int anoLetivo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesRegenciaPorAno(int anoTurma)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AlunoPorTurmaResposta>> ObterDadosAluno(string codidoAluno, int anoLetivo, bool consideraHistorico, bool filtrarSituacao = true)
        {
            throw new NotImplementedException();
        }

        public Task<DadosTurmaEolDto> ObterDadosTurmaPorCodigo(string codigoTurma)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasParaPlanejamento(long codigoTurma, string login, Guid perfil)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurma(string codigoTurma)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIdsAgrupadas(long[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIdsSemAgrupamento(long[] ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DreRespostaEolDto> ObterDres()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EscolasRetornoDto> ObterEscolasPorCodigo(string[] codigoUes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EscolasRetornoDto> ObterEscolasPorDre(string dreId)
        {
            throw new NotImplementedException();
        }

        public EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorDre()
        {
            throw new NotImplementedException();
        }

        public EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorTurma(string[] codigosTurma = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UsuarioEolRetornoDto> ObterFuncionariosPorCargoUe(string ueId, long cargoId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorCargoUeAsync(string ueId, long cargoId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorDre(Guid perfil, FiltroFuncionarioDto filtroFuncionariosDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorUe(BuscaFuncionariosFiltroDto buscaFuncionariosFiltroDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProfessorResumoDto>> ObterListaNomePorListaRF(IEnumerable<string> codigosRF)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProfessorResumoDto>> ObterListaResumosPorListaRF(IEnumerable<string> codigosRF, int anoLetivo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf)
        {
            throw new NotImplementedException();
        }

        public Task<MeusDadosDto> ObterMeusDados(string login)
        {
            throw new NotImplementedException();
        }

        public Task<InformacoesEscolaresAlunoDto> ObterNecessidadesEspeciaisAluno(string codigoAluno)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObterNomeProfessorPeloRF(string rfProfessor)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioEolAutenticacaoRetornoDto> ObterPerfisPorLogin(string login)
        {
            throw new NotImplementedException();
        }

        public Task<int[]> ObterPermissoesPorPerfil(Guid perfilGuid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string ueId, string nomeProfessor)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string nomeProfessor, bool incluirEmei)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesDisciplinas(string turmaCodigo, DateTime? dataReferencia = null, string professorRf = null, bool realizaAgrupamento = true)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesPorTurmas(IEnumerable<string> codigosTurmas)
        {
            throw new NotImplementedException();
        }

        public Task<ProfessorResumoDto> ObterProfessorPorRFUeDreAnoLetivo(string codigoRF, int anoLetivo, string dreId, string ueId)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioResumoCoreDto> ObterResumoCore(string login)
        {
            throw new NotImplementedException();
        }

        public Task<ProfessorResumoDto> ObterResumoProfessorPorRFAnoLetivo(string codigoRF, int anoLetivo, bool buscarOutrosCargos = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorCodigo(string[] codigoSupervisores)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorDre(string dreId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopiaPlanoAnual(string codigoRf, long componenteCurricularId, int codigoTurma)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TurmaPorUEResposta>> ObterTurmasPorUE(string ueId, string anoLetivo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PodePersistirTurma(string professorRf, string codigoTurma, DateTime data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PodePersistirTurmaDisciplina(string professorRf, string codigoTurma, string disciplinaId, DateTime data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PodePersistirTurmaNoPeriodo(string professorRf, string codigoTurma, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ProfessorPodePersistirTurma(string professorRf, string codigoTurma, DateTime data)
        {
            throw new NotImplementedException();
        }

        public Task ReiniciarSenha(string login)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioEolAutenticacaoRetornoDto> RelecionarUsuarioPerfis(string login)
        {
            throw new NotImplementedException();
        }

        public Task RemoverCJSeNecessario(Guid usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TurmaPossuiComponenteCurricularPAP(string codigoTurma, string login, Guid idPerfil)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidarProfessor(string professorRf)
        {
            throw new NotImplementedException();
        }

        public Task<AtribuicaoProfessorTurmaEOLDto> VerificaAtribuicaoProfessorTurma(string professorRf, string codigoTurma)
        {
            throw new NotImplementedException();
        }
    }
}
