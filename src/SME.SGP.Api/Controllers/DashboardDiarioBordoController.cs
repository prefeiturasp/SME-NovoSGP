using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v1/dashboard/diario-bordo")]
    public class DashboardDiarioBordoController : Controller
    {
        [HttpGet("quantidade-total-diarios-e-turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoTotalDiariosPendentesDTO>), 200)]
        [Permissao(Permissao.DB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurma([FromQuery] FiltroDasboardDiarioBordoDto filtro, [FromServices] IObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("quantidade-diarios-pendentes")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoTotalDiariosEDevolutivasDTO>), 200)]
        [Permissao(Permissao.DB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeTotalDeDiariosPendentesPorAnoTurma([FromQuery] FiltroDasboardDiarioBordoDto filtro, [FromServices] IObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("quantidade-diarios-pendentes-dre")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoTotalDiariosEDevolutivasPorDreDTO>), 200)]
        [Permissao(Permissao.DB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeTotalDeDiariosPendentesPorDre([FromQuery]int anoLetivo, string ano, [FromServices] IObterQuantidadeTotalDeDiariosPendentesPorDREUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, ano));
        }

        [HttpGet("consolidacao")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterUltimaConsolidacao([FromQuery] int anoLetivo, [FromServices] IObterUltimaConsolidacaoDiarioBordoUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo));
        }

    }
}
