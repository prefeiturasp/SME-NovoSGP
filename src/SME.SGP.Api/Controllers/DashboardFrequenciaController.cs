using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v1/dashboard/frequencias")]
    public class DashboardFrequenciaController : Controller
    {

        [HttpGet("modalidades/ano")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> ModalidadesPorAno([FromQuery] int anoLetivo, long dreId, long ueId, int modalidade, int semestre, [FromServices] IObterModalidadesAnoUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade, semestre));
        }

        [HttpGet("global/por-ano")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar(int anoLetivo, long dreId, long ueId, Modalidade modalidade, int semestre, [FromServices] IObterDashboardFrequenciaPorAnoUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade, semestre));
        }

        [HttpGet("global/dre")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoFrequenciaGlobalPorDREDto>), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaGlobalPorDre(int anoLetivo, [FromServices] IObterDadosDashboardFrequenciaPorDreUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo));
        }
    }
}
