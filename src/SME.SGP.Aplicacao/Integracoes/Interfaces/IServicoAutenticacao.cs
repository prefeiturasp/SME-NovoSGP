using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoAutenticacao
    {
        Task<(UsuarioAutenticacaoRetornoDto, string)> AutenticarNoEol(string login, string senha);
    }
}