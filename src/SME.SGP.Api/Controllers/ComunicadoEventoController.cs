using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/comunicadoevento")]
    [Authorize("Bearer")]
    public class ComunicadoEventoController : ControllerBase
    {
        [HttpPost("ListarPorCalendario")]
        [ProducesResponseType(typeof(IEnumerable<ListarEventosPorCalendarioRetornoDto>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ListarEventosPorCalendarioRetornoDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ListarEventosPorCalendario([FromBody] ListarEventoPorCalendarioDto filtro, [FromServices] IListarEventosPorCalendarioUseCase useCase)
        {
            var retorno = await useCase.Executar(filtro);

            if (retorno == null || !retorno.Any())
                return NoContent();

            return Ok(retorno);
        }
    }
}