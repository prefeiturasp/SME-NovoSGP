using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUsuario : IRepositorioBase<Usuario>
    {
        Usuario ObterPorCodigoRfLogin(string codigoRf, string login);

        Usuario ObterPorTokenRecuperacaoSenha(Guid token);
        Task<Usuario> ObterUsuarioPorCodigoRfAsync(string codigoRf);
    }
}