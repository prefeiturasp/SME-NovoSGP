using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/menus")]
    [Authorize("Bearer")]
    public class MenuController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<MenuRetornoDto>), 200)]
        public async Task<IActionResult> Get([FromServices]IServicoMenu servicoMenu)
        {
            return Ok(await servicoMenu.ObterMenu());
        }
    }
}