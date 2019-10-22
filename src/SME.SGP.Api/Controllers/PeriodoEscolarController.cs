using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/periodo-escolar")]
    [ValidaDto]
    public class PeriodoEscolarController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(PeriodoEscolarDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public IActionResult Get(long codigoTipoCalendario, [FromServices]IConsultasPeriodoEscolar consultas)
        {
            var periodoEscolar = consultas.ObterPorTipoCalendario(codigoTipoCalendario);

            if (periodoEscolar == null)
                return NoContent();

            return Ok(periodoEscolar);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Permissao.PA_A, Policy = "Bearer")]
        public IActionResult Post([FromBody]PeriodoEscolarListaDto periodos, [FromServices]IComandosPeriodoEscolar comandoPeriodo)
        {
            comandoPeriodo.Salvar(periodos);
            return Ok();
        }
    }
}