using System;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAutenticacao
    {
        string RecuperarSenha(string usuario);

        bool TokenRecuperacaoSenhaEstaValido(Guid token);
    }
}