﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v1/dashboard/registros_individuais")]
    public class DashboardRegistroIndividualController : Controller
    {
        [HttpGet("total-ano-turma")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoTotalDiariosPendentesDTO>), 200)]
        [Permissao(Permissao.DRIN_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeRegistrosIndividuaisPorAnoTurma([FromQuery] FiltroDasboardRegistroIndividualDTO filtro, [FromServices] IObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("media")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoBaseDto>), 200)]
        [Permissao(Permissao.DRIN_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDadosDashboard([FromQuery] FiltroDasboardRegistroIndividualDTO filtro, [FromServices] IObterDadosDashboardRegistrosIndividuaisUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }        
        
        [HttpGet("ultima-consolidacao")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(DateTime?), 200)]
        [Permissao(Permissao.DRIN_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterUltimaConsolidacao([FromQuery] int anoLetivo, [FromServices] IObterUltimaConsolidacaoMediaRegistrosIndividuaisUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo));
        }

        [HttpGet("alunos-sem-registro")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoBaseDto>), 200)]
        [Permissao(Permissao.DRIN_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDadosAlunosSemRegistro([FromQuery] FiltroDasboardRegistroIndividualDTO filtro, [FromServices] IObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
        
        [HttpGet("quantidade-dias-sem-registro")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoBaseDto>), 200)]
        [Permissao(Permissao.DRIN_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeDiasSemRegistro(int anoLetivo, [FromServices] IObterParametroDiasSemRegistroIndividualUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo));
        }
        
        [HttpGet("dre")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoBaseDto>), 200)]
        [Permissao(Permissao.DRIN_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTotalRIsPorDRE([FromQuery] FiltroDashboardTotalRIsPorDreDTO filtro, [FromServices] IObterTotalRIsPorDreUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }
}
