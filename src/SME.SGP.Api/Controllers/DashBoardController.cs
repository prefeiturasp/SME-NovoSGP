using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/dashboard")]
    [Authorize("Bearer")]
    public class DashBoardController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DashBoard>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Obter([FromServices] IObterDashBoardUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }
    }
}
