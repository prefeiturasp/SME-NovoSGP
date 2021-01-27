using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/pendencias")]
    public class PendenciasController : ControllerBase
    {
        [HttpGet()]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<PendenciaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Listar([FromServices]IObterPendenciasUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }
    }
}
