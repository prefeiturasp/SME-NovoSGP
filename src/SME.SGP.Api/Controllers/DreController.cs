using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/dres")]
    [Authorize("Bearer")]
    public class DreController : ControllerBase
    {
        [HttpGet("atribuicoes")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaDreRetornoDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterDresAtribuicoes([FromQuery] int anoLetivo, [FromServices] IConsultasAtribuicoes consultasAtribuicoes)
        {
            IEnumerable<AbrangenciaDreRetornoDto> dres = await consultasAtribuicoes.ObterDres(anoLetivo);

            return Ok(dres);
        }
    }
}