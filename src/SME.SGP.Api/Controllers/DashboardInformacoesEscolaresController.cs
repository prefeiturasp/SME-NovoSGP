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
    //[Authorize("Bearer")]
    [Route("api/v1/dashboard/informacoes-escolares")]
    public class DashboardInformacoesEscolaresController : Controller
    {
        [HttpGet("matriculas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoBaseDto>), 200)]
        //[Permissao(Permissao.PDA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaGlobalPorDre([FromQuery] FiltroGraficoMatriculaDto filtro, [FromServices] IObterDashboardMatriculaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }
}
