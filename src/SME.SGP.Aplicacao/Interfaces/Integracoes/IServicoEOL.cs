using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoEOL
    {
        Task<AlterarSenhaRespostaDto> AlterarSenha(string login, string novaSenha);

        Task<UsuarioEolAutenticacaoRetornoDto> Autenticar(string login, string senha);

        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasParaPlanejamento(long codigoTurma, string rfProfessor);

        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurmaLoginEPerfil(long codigoTurma, string login, string guidPerfil);

        IEnumerable<DreRespostaEolDto> ObterDres();

        IEnumerable<EscolasRetornoDto> ObterEscolasPorCodigo(string[] codigoUes);

        IEnumerable<EscolasRetornoDto> ObterEscolasPorDre(string dreId);

        IEnumerable<UsuarioEolRetornoDto> ObterFuncionariosPorCargoUe(string UeId, long cargoId);

        Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorUe(BuscaFuncionariosFiltroDto buscaFuncionariosFiltroDto);

        IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf);

        Task<MeusDadosDto> ObterMeusDados(string login);

        Task<UsuarioEolAutenticacaoRetornoDto> ObterPerfisPorLogin(string login);

        Task<int[]> ObterPermissoesPorPerfil(Guid perfilGuid);

        IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorCodigo(string[] codigoSupervisores);

        IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorDre(string dreId);

        Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo);

        Task ReiniciarSenha(string login);
    }
}