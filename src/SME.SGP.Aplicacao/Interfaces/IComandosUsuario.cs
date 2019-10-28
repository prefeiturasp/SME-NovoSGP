using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosUsuario
    {
        Task AlterarEmail(AlterarEmailDto alterarEmailDto, string codigoRf);

        Task AlterarEmailUsuarioLogado(string novoEmail);

        Task AlterarSenha(AlterarSenhaDto alterarSenhaDto);

        Task AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto);

        Task<AlterarSenhaRespostaDto> AlterarSenhaPrimeiroAcesso(PrimeiroAcessoDto primeiroAcessoDto);

        Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha);

        Task<string> ModificarPerfil(string guid);

        Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf);

        string SolicitarRecuperacaoSenha(string login);

        bool TokenRecuperacaoSenhaEstaValido(Guid token);
        void Sair();
    }
}