using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/parecer-conclusivo")]
    [Authorize("Bearer")]
    public class RelatorioParecerConclusivoController : ControllerBase
    {
        [HttpGet("imprimir")]
        public async Task<IActionResult> Imprimir([FromQuery] FiltroRelatorioParecerConclusivoDto filtroRelatorioParecerConclusivoDto, [FromServices]IImpressaoRelatorioParecerConclusivoUseCase impressaoUseCase)
        {
            return Ok(await impressaoUseCase.Executar(filtroRelatorioParecerConclusivoDto));
        }
    }
}
