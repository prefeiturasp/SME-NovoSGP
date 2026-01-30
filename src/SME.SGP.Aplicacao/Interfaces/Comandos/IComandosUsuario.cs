using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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
        Task<UsuarioAutenticacaoRetornoDto> AutenticarSSO(string login, string senha);

        Task<TrocaPerfilDto> ModificarPerfil(Guid perfil);

        Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf);

        Task<RevalidacaoTokenDto> RevalidarLogin();

        void Sair();

        Task<string> SolicitarRecuperacaoSenha(string login);

        Task<bool> TokenRecuperacaoSenhaEstaValido(Guid token);

        Task<UsuarioAutenticacaoRetornoDto> AutenticarSuporte(string login);

        Task<UsuarioAutenticacaoRetornoDto> ObterAutenticacao(
            (UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ) retornoAutenticacaoEol,
            string login, SuporteUsuario suporte = null);
    }
}