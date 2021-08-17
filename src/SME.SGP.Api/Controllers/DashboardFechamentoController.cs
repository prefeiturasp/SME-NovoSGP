using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/dashboard/fechamentos")]    
    public class DashboardFechamentoController : ControllerBase
    {
        [HttpGet("situacoes")]
        [ProducesResponseType(typeof(FechamentoSituacaoDto), 200)]
        [ProducesResponseType(typeof(FechamentoSituacaoDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSituacoesFechamento(
            [FromQuery] FiltroDashboardFechamentoDto filtroDashboardFechamentoDto,
            [FromServices] IObterFechamentoSituacaoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtroDashboardFechamentoDto));
        }
    }
}