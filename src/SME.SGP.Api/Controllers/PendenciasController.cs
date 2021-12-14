using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/pendencias")]
    [Authorize("Bearer")]
    public class PendenciasController : ControllerBase
    {
        [HttpGet()]
        [Route("listar")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<PendenciaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Listar([FromQuery] FiltroPendenciasUsuarioDto filtro, 
                                                [FromServices] IObterPendenciasUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }
}