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
    [Route("api/v1/comunicados/eventos")]
    [Authorize("Bearer")]
    public class ComunicadoEventoController : ControllerBase
    {
        [HttpPost("")]
        [ProducesResponseType(typeof(IEnumerable<EventoCalendarioRetornoDto>), 200)]
        [ProducesResponseType(typeof(IEnumerable<EventoCalendarioRetornoDto>), 204)]
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