﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ues")]
    [Authorize("Bearer")]
    public class UeController : ControllerBase
    {
        [HttpGet("{codigoUe}/modalidades")]
        [ProducesResponseType(typeof(IEnumerable<ModalidadeRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterModalidedes([FromServices] IObterModalidadesPorUeUseCase obterModalidadesPorUeUseCase, string codigoUe, [FromQuery]int ano, [FromQuery] bool consideraNovasModalidades = false)
        {
            return Ok(await obterModalidadesPorUeUseCase.Executar(codigoUe, ano, consideraNovasModalidades));
        }

        [HttpGet("{codigoUe}/modalidades/{idModalidade}")]
        [ProducesResponseType(typeof(IEnumerable<TurmaRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTurmas(string codigoUe, int idModalidade, [FromQuery]int ano, [FromServices]IConsultasUe consultasUe)
        {
            return Ok(await consultasUe.ObterTurmas(codigoUe, idModalidade, ano));
        }

        [HttpGet("/api/v1/dres/{codigoDre}/ues/atribuicoes")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaUeRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterUesAtribuicoes(string codigoDre, [FromServices] IConsultasAtribuicoes consultasAtribuicoes)
        {
            return Ok(await consultasAtribuicoes.ObterUes(codigoDre));
        }
    }
}