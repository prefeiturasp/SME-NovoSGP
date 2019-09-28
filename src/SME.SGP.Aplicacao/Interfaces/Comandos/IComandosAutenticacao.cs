using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAutenticacao
    {
        Task AlterarSenhaComTokenRecuperacao(Guid token, string novaSenha);

        string SolicitarRecuperacaoSenha(string login);

        bool TokenRecuperacaoSenhaEstaValido(Guid token);
    }
}