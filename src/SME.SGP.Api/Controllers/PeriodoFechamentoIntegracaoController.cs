using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/periodos/fechamentos/aberturas")]
    [ChaveIntegracaoSgpApi]
    [ValidaDto]
    public class PeriodoFechamentoIntegracaoController : ControllerBase
    {
        [HttpGet("vigente")]
        [ProducesResponseType(typeof(PeriodoFechamentoVigenteDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Get([FromQuery] FiltroPeriodoFechamentoVigenteDto filtroDto, [FromServices] IObterPeriodoFechamentoVigenteUseCase useCase)
        {
            return Ok(await useCase.Executar(filtroDto));
        }
    }
}
