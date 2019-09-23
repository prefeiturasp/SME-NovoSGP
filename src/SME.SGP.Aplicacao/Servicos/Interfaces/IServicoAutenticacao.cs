using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public interface IServicoAutenticacao
    {
        Task<UsuarioAutenticacaoRetornoDto> AutenticarNoEol(string login, string senha);
    }
}