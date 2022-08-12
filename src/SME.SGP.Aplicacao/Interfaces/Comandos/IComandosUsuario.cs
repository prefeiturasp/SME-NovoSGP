using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosUsuario
    {
        Task AlterarEmail(AlterarEmailDto alterarEmailDto, string codigoRf);

        Task AlterarEmailUsuarioLogado(string novoEmail);

        Task AlterarSenha(AlterarSenhaDto alterarSenhaDto);

        Task<UsuarioAutenticacaoRetornoDto> AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto);

        Task<AlterarSenhaRespostaDto> AlterarSenhaPrimeiroAcesso(PrimeiroAcessoDto primeiroAcessoDto);

        Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha);

        Task<TrocaPerfilDto> ModificarPerfil(Guid perfil);

        Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf);

        Task<RevalidacaoTokenDto> RevalidarLogin();

        void Sair();

        Task<string> SolicitarRecuperacaoSenha(string login);

        Task<bool> TokenRecuperacaoSenhaEstaValido(Guid token);

        Task<UsuarioAutenticacaoRetornoDto> AutenticarSuporte(string login);

        Task<UsuarioAutenticacaoRetornoDto> ObtenhaAutenticacao((UsuarioAutenticacaoRetornoDto, string, IEnumerable<Guid>, bool, bool) retornoAutenticacaoEol, string login, List<Claim> claims = null);
    }
}