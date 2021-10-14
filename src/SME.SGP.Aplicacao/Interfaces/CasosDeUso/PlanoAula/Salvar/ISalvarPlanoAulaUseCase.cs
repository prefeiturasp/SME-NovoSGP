using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarPlanoAulaUseCase
    {
        Task<PlanoAulaDto> Executar(PlanoAulaDto planoAulaDto);
    }
}