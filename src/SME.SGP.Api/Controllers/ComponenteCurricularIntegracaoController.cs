using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/componente-curricular/integracoes")]
    [ChaveIntegracaoSgpApi]
    [ValidaDto]
    public class ComponenteCurricularIntegracaoController : ControllerBase
    {
        [HttpGet("lanca-nota")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> VerificarComponenteLancaNota(long componenteCurricularId, [FromServices] IObterComponenteCurricularLancaNotaUseCase useCase)
        {
            return Ok(await useCase.Executar(componenteCurricularId));
        }
    }
}
