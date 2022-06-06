using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Autenticacao
{
    public interface IDeslogarSuporteUsuarioUseCase 
    {
        Task<UsuarioAutenticacaoRetornoDto> Executar();
    }
}
