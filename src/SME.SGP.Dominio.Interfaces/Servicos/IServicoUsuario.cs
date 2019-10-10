using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoUsuario
    {
        Task AlterarEmailUsuarioPorLogin(string login, string novoEmail);

        Task AlterarEmailUsuarioPorRfOuInclui(string codigoRf, string novoEmail);

        void ModificarPerfil(string perfilParaModificar, string login);

        Usuario ObterUsuarioPorCodigoRfLoginOuAdiciona(string codigoRf, string login = "");

        Task PodeModificarPerfil(string perfilParaModificar, string login);
    }
}