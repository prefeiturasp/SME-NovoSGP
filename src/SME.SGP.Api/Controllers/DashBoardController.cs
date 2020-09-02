using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/dashBoard")]
    [Authorize("Bearer")]
    public class DashBoardController : ControllerBase
    {
        [HttpGet("{idPerfil}")]
        [ProducesResponseType(typeof(IEnumerable<DashBoardRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Obter([FromServices] IObterDashBoardUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }
    }
}
