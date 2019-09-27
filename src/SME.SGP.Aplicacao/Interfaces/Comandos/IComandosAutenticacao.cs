using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAutenticacao
    {
        Task AlterarSenha(Guid token, string novaSenha);

        string RecuperarSenha(string usuario);

        bool TokenRecuperacaoSenhaEstaValido(Guid token);
    }
}