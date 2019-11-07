using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUsuario : IRepositorioBase<Usuario>
    {
        bool ExisteUsuarioComMesmoEmail(string email, long idUsuarioExistente);

        Usuario ObterPorCodigoRfLogin(string codigoRf, string login);

        Usuario ObterPorTokenRecuperacaoSenha(Guid token);
    }
}