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
    [Route("api/v1/dashboard/informacoes-escolares")]
    public class DashboardInformacoesEscolaresController : Controller
    {
        [HttpGet("matriculas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoBaseDto>), 200)]
        [Permissao(Permissao.DIE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterGraficoPorMatricula([FromQuery] FiltroGraficoMatriculaDto filtro, [FromServices] IObterDashboardInformacoesEscolaresPorMatriculaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoBaseDto>), 200)]
        [Permissao(Permissao.DIE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterGraficoPorTurma([FromQuery] FiltroGraficoMatriculaDto filtro, [FromServices] IObterDashboardInformacoesEscolaresPorTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("ultima-consolidacao")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(DateTime), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 204)]
        [Permissao(Permissao.DIE_C, Policy = "Bearer")]
        public async Task<IActionResult> UltimaConsolidacao(int anoLetivo, [FromServices] IObterDataConsolidacaoInformacoesEscolaresUseCase useCase)
        {
            var ultimaConsolidacao = await useCase.Executar(anoLetivo);

            if (!ultimaConsolidacao.HasValue)
                return NoContent();

            return Ok(ultimaConsolidacao.Value);
        }

        [HttpGet("modalidades/anos")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [Permissao(Permissao.DIE_C, Policy = "Bearer")]
        public async Task<IActionResult> ModalidadesPorAno([FromQuery] int anoLetivo, long dreId, long ueId, int modalidade, int semestre, [FromServices] IObterModalidadeAnoItineranciaProgramaUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade, semestre));
        }
    }
}
