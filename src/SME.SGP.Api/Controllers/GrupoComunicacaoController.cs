﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/comunicacao/grupos/")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class GrupoComunicacaoController : ControllerBase
    {
        private readonly IConsultaGrupoComunicacao consultaGrupoComunicacao;

        public GrupoComunicacaoController(IConsultaGrupoComunicacao consultaGrupoComunicacao)
        {
            this.consultaGrupoComunicacao = consultaGrupoComunicacao ?? throw new System.ArgumentNullException(nameof(consultaGrupoComunicacao));
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(IEnumerable<GrupoComunicacaoCompletoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public async Task<IActionResult> BuscarTodosAsync([FromQuery]FiltroGrupoComunicacaoDto filtro)
        {
            return Ok(await consultaGrupoComunicacao.Listar(filtro));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GrupoComunicacaoCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public async Task<IActionResult> ObterPorId(long id)
        {
            return Ok(await consultaGrupoComunicacao.ObterPorIdAsync(id));
        } 

        [HttpGet("listar/modalidade/{modalidade}")]
        [ProducesResponseType(typeof(IEnumerable<long>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [AllowAnonymous]
        public async Task<IActionResult> ListarIdsPorModalidade(Modalidade modalidade, [FromServices]IConsultaGrupoComunicacao consultaGrupoComunicacao)
        {
            return Ok(await consultaGrupoComunicacao.ObterIdsGrupoComunicadoPorModalidade(modalidade));
        }
    }
}