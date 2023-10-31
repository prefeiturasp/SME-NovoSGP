﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class DiasLetivosCalendarioController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(typeof(DiasLetivosDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("dias-letivos")]
        [Permissao(Permissao.C_C, Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> CalcularDiasLetivos(FiltroDiasLetivosDTO filtro, [FromServices] IObterDiasLetivosPorCalendarioUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }
}