using SME.SGP.Dto;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosUsuario
    {
        Task AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto);

        Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha);

        Task<string> ModificarPerfil(string guid);

        string SolicitarRecuperacaoSenha(string login);

        void TesteEmail();

        bool TokenRecuperacaoSenhaEstaValido(Guid token);
    }
}