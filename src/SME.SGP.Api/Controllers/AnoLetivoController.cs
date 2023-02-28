using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/anos-letivos")]
    [Authorize("Bearer")]
    public class AnoLetivoController : ControllerBase
    {
        [HttpGet("atribuicoes")]
        [ProducesResponseType(typeof(IEnumerable<int>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> ObterAnosLetivosAtribuicoes([FromQuery] bool consideraHistorico, [FromServices] IConsultasAtribuicoes consultasAtribuicoes)
        {
            return Ok(await consultasAtribuicoes.ObterAnosLetivos(consideraHistorico));
        }
    }
}
