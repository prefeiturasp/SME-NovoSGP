using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/periodos/fechamentos/aberturas")]
    public class FechamentoController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(FechamentoDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.PFA_C, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromQuery]FiltroFechamentoDto fechamentoDto, [FromServices] IConsultasFechamento consultasFechamento)
        {
            return Ok(await consultasFechamento.ObterPorTipoCalendarioDreEUe(fechamentoDto));
        }
    }
}