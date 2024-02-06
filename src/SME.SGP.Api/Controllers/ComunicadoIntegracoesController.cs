using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/comunicados/integracoes")]
    [ApiController]
    [ChaveIntegracaoSgpApi]
    public class ComunicadoIntegracoesController : ControllerBase
    {
        [HttpGet("ano-atual")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTodosAnoAtual([FromServices] IObterComunicadosAnoAtualUseCase useCase)
        {
            var comunicados = await useCase.Executar();
            return Ok(comunicados);
        }
    }
}
