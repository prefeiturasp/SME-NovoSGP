using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/periodos/fechamentos/aberturas")]
    public class PeriodoFechamentoController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Get([FromServices] IConsultasPeriodoFechamento consultasPeriodoFechamento)
        {
            return Ok(consultasPeriodoFechamento.GetTeste());
        }
    }
}