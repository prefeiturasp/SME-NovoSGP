using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v1/dashboard/diario-bordo")]
    public class DashboardDiarioBordoController : Controller
    {
        [HttpGet("quantidade-total-diarios-e-turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoBaseDto>), 200)]
        //[Permissao(Permissao.DB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurma([FromQuery] FiltroDasboardDiarioBordoDto filtro, [FromServices] IObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }
}
