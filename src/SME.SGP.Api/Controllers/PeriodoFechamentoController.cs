using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/periodos/fechamento/abertura")]
    [Authorize("Bearer")]
    public class PeriodoFechamentoController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Get(IConsultasPeriodoFechamento consultasPeriodoFechamento)
        {
            return Ok(consultasPeriodoFechamento.GetTeste());
        }
    }
}