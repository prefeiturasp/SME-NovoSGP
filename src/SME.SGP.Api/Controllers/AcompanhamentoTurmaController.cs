﻿using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/acompanhamento/turmas")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class AcompanhamentoTurmaController : Controller
    {
        [HttpPost("")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Salvar([FromServices] ISalvarAcompanhamentoTurmaUseCase useCase, [FromBody] AcompanhamentoTurmaDto dto)
             => Ok(await useCase.Executar(dto));


        [HttpGet("apanhado-geral")]
        [ProducesResponseType(typeof(AcompanhamentoTurmaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        public async Task<IActionResult> Obter([FromQuery] FiltroAcompanhamentoTurmaApanhadoGeral dto, [FromServices] IObterAcompanhamentoTurmaApanhadoGeralUseCase useCase)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpGet("quantidade-imagens")]
        [ProducesResponseType(typeof(ParametroQuantidadeUploadImagemDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        public async Task<IActionResult> ObterParametroQuantidadeImagens([FromQuery] int ano, [FromServices] IObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(ano));
        }
    }
}
