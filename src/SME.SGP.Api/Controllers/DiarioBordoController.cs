using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/diario-bordo")]
    [Authorize("Bearer")]
    public class DiarioBordoController : ControllerBase
    {

        [HttpGet("{aulaId}")]
        [ProducesResponseType(typeof(DiarioBordoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter(long aulaId, [FromServices] IMediator mediator)
        {
            return Ok(await ObterDiarioBordoPorAulaIdUseCase.Executar(mediator, aulaId));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao., Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromServices]IInserirDiarioBordoUseCase useCase, [FromBody]InserirDiarioBordoDto diarioBordoDto)
        {
            return Ok(await useCase.Executar(diarioBordoDto));
        }
    }
}
