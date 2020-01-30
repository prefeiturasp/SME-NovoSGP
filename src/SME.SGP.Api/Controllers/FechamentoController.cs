using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/fechamentos")]
    [ValidaDto]
    public class FechamentoController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult RealizarFechamento([FromQuery]string codigoTurma, [FromQuery]string disciplinaId, [FromQuery] long periodoEscolarId, [FromServices]IServicoFechamento servicoFechamento)
        {
            servicoFechamento.RealizarFechamento(codigoTurma, disciplinaId, periodoEscolarId);
            return Ok();
        }
    }
}