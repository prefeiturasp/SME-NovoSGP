using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosUsuario
    {
        Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha);

        Task<string> ModificarPerfil(string guid);
    }
}