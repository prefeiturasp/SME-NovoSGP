using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAutenticacao
    {
        Task<UsuarioAutenticacaoRetornoDto> AutenticarNoEol(string login, string senha);
    }
}