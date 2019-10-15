using SME.SGP.Dto;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosUsuario
    {
        Task AlterarEmail(AlterarEmailDto alterarEmailDto, string codigoRf);

        Task AlterarEmailUsuarioLogado(string novoEmail);

        Task AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto);

        Task<AlterarSenhaRespostaDto> AlterarSenhaPrimeiroAcesso(PrimeiroAcessoDto primeiroAcessoDto);

        Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha);

        Task<string> ModificarPerfil(Guid perfil);

        Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf);

        string SolicitarRecuperacaoSenha(string login);

        bool TokenRecuperacaoSenhaEstaValido(Guid token);
    }
}