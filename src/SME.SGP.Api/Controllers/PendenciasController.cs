﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/pendencias")]
    [Authorize("Bearer")]
    public class PendenciasController : ControllerBase
    {
        [HttpGet()]
        [Route("listar")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<PendenciaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Listar([FromServices] IObterPendenciasUseCase useCase,
            [FromQuery] string turmaCodigo = null, int tipoPendencia = 0, string tituloPendencia = null)
        {
            return Ok(await useCase.Executar(turmaCodigo, tipoPendencia, tituloPendencia));
        }
    }
}