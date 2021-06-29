using Microsoft.AspNetCore.Authorization;
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
    [Route("api/v1/dashboard/acompanhamento_aprendizagem")]
    public class DashboardAcompanhamentoAprendizagemController : Controller
    {
        
        [HttpGet("ultima-consolidacao")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(DateTime?), 200)]
        //[Permissao(Permissao.DRIN_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterUltimaConsolidacao([FromQuery] int anoLetivo, [FromServices] IObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo));
        }
    }
}
