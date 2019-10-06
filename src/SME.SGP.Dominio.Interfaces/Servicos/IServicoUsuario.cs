using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoUsuario
    {
        Task AlterarEmail(string login, string novoEmail);

        Usuario ObterUsuarioPorCodigoRfLoginOuAdiciona(string codigoRf, string login = "");

        Task PodeModificarPerfil(string perfilParaModificar, string login);
    }
}