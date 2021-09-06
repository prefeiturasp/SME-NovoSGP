using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/aulas_infantil")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class AulaInfantilController : Controller
    {
        [HttpPost("sincronizar-aulas")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SincronizarAulasTurma([FromQuery, Required] long codigoTurma, [FromServices] ISincronizarAulasInfantilUseCase sincronizarAulasInfantilUseCase)
        {
            return Ok(await sincronizarAulasInfantilUseCase.Executar(codigoTurma));
        }
    }
}
