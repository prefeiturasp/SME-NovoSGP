using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v1/dashboard/busca-ativa")]
    public class DashboardBuscaAtivaController : ControllerBase
    {
        [HttpGet("motivos-ausencia")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoBuscaAtivaDto>), 200)]
        [Permissao(Permissao.DBA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeBuscaAtivaPorMotivosAusencia([FromQuery] FiltroGraficoBuscaAtivaDto filtro, [FromServices] IObterQuantidadeBuscaAtivaPorMotivosAusenciaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("procedimentos-trabalho")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoBuscaAtivaDto>), 200)]
        [Permissao(Permissao.DBA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDre([FromQuery] FiltroGraficoProcedimentoTrabalhoBuscaAtivaDto filtro, [FromServices] IObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDreUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }
}
