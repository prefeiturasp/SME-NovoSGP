using SME.SGP.Dto;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosUsuario
    {
        void AlterarEmail(AlterarEmailDto alterarEmailDto);

        Task AlterarEmailUsuarioLogado(string novoEmail);

        Task AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto);

        Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha);

        Task<string> ModificarPerfil(string guid);

        Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string login);

        string SolicitarRecuperacaoSenha(string login);

        bool TokenRecuperacaoSenhaEstaValido(Guid token);
    }
}