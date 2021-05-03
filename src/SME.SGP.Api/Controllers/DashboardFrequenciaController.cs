using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    //[Authorize("Bearer")]
    [Route("api/v1/dashboard/frequencias")]
    public class DashboardFrequenciaController : Controller
    {
        [HttpGet("global/por-ano")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        //[Permissao(Permissao.PDA_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar(int anoLetivo, long dreId, long ueId, Modalidade modalidade, [FromServices] IObterDashboardFrequenciaPorAnoUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade));
        }
    }
}
