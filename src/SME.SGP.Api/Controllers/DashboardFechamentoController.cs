﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/dashboard/fechamentos")]
    [Authorize("Bearer")]
    public class DashboardFechamentoController : ControllerBase
    {
        [HttpGet("situacoes")]
        [ProducesResponseType(typeof(FechamentoSituacaoDto), 200)]
        [ProducesResponseType(typeof(FechamentoSituacaoDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DFE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSituacoesFechamento([FromQuery] FiltroDashboardFechamentoDto filtroDashboardFechamentoDto, [FromServices] IObterFechamentoSituacaoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtroDashboardFechamentoDto));
        }

        [HttpGet("estudantes")]
        [ProducesResponseType(typeof(FechamentoSituacaoPorEstudanteDto), 200)]
        [ProducesResponseType(typeof(FechamentoSituacaoPorEstudanteDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DFE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSituacoesFechamentoPorEstudante(
            [FromQuery] FiltroDashboardFechamentoDto filtroDashboardFechamentoDto,
            [FromServices] IObterFechamentoSituacaoPorEstudanteUseCase useCase)
        {
            return Ok(await useCase.Executar(filtroDashboardFechamentoDto));
        }

        [HttpGet("pendencias")]
        [ProducesResponseType(typeof(GraficoBaseDto), 200)]
        [ProducesResponseType(typeof(GraficoBaseDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DFE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPendenciaFechamento(
            [FromQuery] FiltroDashboardFechamentoDto filtroDashboardFechamentoDto,
            [FromServices] IObterFechamentoPendenciasUseCase useCase)
        {
            return Ok(await useCase.Executar(filtroDashboardFechamentoDto));
        }

        [HttpGet("conselhos-classes/situacoes")]
        [ProducesResponseType(typeof(GraficoBaseDto), 200)]
        [ProducesResponseType(typeof(GraficoBaseDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DFE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSituacoesConselhoClasse([FromQuery]
            FiltroDashboardFechamentoDto filtroDashboardFechamentoDto,
            [FromServices] IObterFechamentoConselhoClasseSituacaoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtroDashboardFechamentoDto));
        }

        [HttpGet("conselhos-classes/notas-finais")]
        [ProducesResponseType(typeof(GraficoBaseDto), 200)]
        [ProducesResponseType(typeof(GraficoBaseDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DFE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPosConselho(
        [FromQuery] FiltroDashboardFechamentoDto filtroDashboardFechamentoDto,
        [FromServices] IObterNotasFinaisUseCases useCase)
        {
            return Ok(await useCase.Executar(filtroDashboardFechamentoDto));
        }

        [HttpGet("conselhos-classes/pareceres-conclusivos")]
        [ProducesResponseType(typeof(GraficoBaseDto), 200)]
        [ProducesResponseType(typeof(GraficoBaseDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DFE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPendenciaParecerConclusivo(
        [FromQuery] FiltroDashboardFechamentoDto filtroDashboardFechamentoDto,
        [FromServices] IObterPendenciaParecerConclusivoUseCases useCase)
        {
            return Ok(await useCase.Executar(filtroDashboardFechamentoDto));
        }        
    }


}