using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v1/dashboard/registro_individual")]
    public class DashboardRegistroIndividualController : Controller
    {
        [HttpGet("total-ano-turma")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoTotalDiariosPendentesDTO>), 200)]
        [Permissao(Permissao.DRIN_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeRegistrosIndividuaisPorAnoTurma([FromQuery] FiltroDasboardRegistroIndividualDTO filtro, [FromServices] IObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("consolidacao")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(string), 200)]
        [Permissao(Permissao.DRIN_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterUltimaConsolidacao([FromQuery] int anoLetivo, [FromServices] IObterUltimaConsolidacaoMediaRegistrosIndividuaisUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo));
        }
    }
}
