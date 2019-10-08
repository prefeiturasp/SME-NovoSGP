using System;
using System.Threading.Tasks;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao {
    public interface IComandosUsuario {
        void AlterarEmail (AlterarEmailDto alterarEmailDto);

        Task AlterarEmailUsuarioLogado (string novoEmail);

        Task AlterarSenhaComTokenRecuperacao (RecuperacaoSenhaDto recuperacaoSenhaDto);

        Task<AlterarSenhaRespostaDto> AlterarSenhaPrimeiroAcesso (PrimeiroAcessoDto primeiroAcessoDto);

        Task<UsuarioAutenticacaoRetornoDto> Autenticar (string login, string senha);

        Task<string> ModificarPerfil (string guid);

        string SolicitarRecuperacaoSenha (string login);

        bool TokenRecuperacaoSenhaEstaValido (Guid token);
    }
}