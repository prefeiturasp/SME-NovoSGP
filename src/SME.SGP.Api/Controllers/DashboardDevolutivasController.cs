using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v1/dashboard/devolutivas")]
    public class DashboardDevolutivasController : Controller
    {
        [HttpGet("consolidacao/turma-ano")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoDevolutivasEstimadasEConfirmadasDto>), 200)]
        //[Permissao(Permissao.DF_C, Policy = "Bearer")] Será feito em uma task específica
        public async Task<IActionResult> ObterDevolutivasEstimadasEConfirmadas([FromQuery] FiltroGraficoDevolutivasEstimadasEConfirmadasDto filtro,
            [FromServices] IObterDevolutivasEstimadasEConfirmadasUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("diarios-bordo/turma-ano")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto>), 200)]
        //[Permissao(Permissao.DF_C, Policy = "Bearer")] Será feito em uma task específica
        public async Task<IActionResult> ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendente([FromQuery] FiltroGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto filtro,
            [FromServices] IObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("diarios-bordo/reflexoes-replanejamentos/turma-ano")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto>), 200)]
        //[Permissao(Permissao.DF_C, Policy = "Bearer")] Será feito em uma task específica
        public async Task<IActionResult> ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendente([FromQuery] FiltroGraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto filtro,
            [FromServices] IObterDiariosDeBordoComESemReflexoesEReplanejamentosUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
        
        [HttpGet("consolidacao")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.DF_C, Policy = "Bearer")] Será feito em uma task específica
        public async Task<IActionResult> ObterUltimaConsolidacao([FromQuery] int anoLetivo, [FromServices] IObterUltimaConsolidacaoDevolutivaUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo));
        }
    }
}