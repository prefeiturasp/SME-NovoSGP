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
        
        [HttpGet("estudantes")]
        [ProducesResponseType(typeof(FechamentoSituacaoPorEstudanteDto), 200)]
        [ProducesResponseType(typeof(FechamentoSituacaoPorEstudanteDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSituacoesFechamentoPorEstudante(
            [FromQuery] FiltroDashboardFechamentoDto filtroDashboardFechamentoDto,
            [FromServices] IObterFechamentoSituacaoPorEstudanteUseCase useCase)
            {
            return Ok(await useCase.Executar(filtroDashboardFechamentoDto));
        }
        
        [HttpGet("pendencias")]
        [ProducesResponseType(typeof(FechamentoPendeciaDto), 200)]
        [ProducesResponseType(typeof(FechamentoPendeciaDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterPendenciaFechamento(
            [FromQuery] FiltroDashboardFechamentoDto filtroDashboardFechamentoDto,
            [FromServices] IObterFechamentoPendenciasUseCase useCase)
        {
            return Ok(await useCase.Executar(filtroDashboardFechamentoDto));
        }
    }
    
    
}