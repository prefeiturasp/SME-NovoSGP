using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/fechamentos/pendencias")]
    [ValidaDto]
    public class PendenciasFechamentosController : ControllerBase
    {
        [HttpGet("listar")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<PendenciaFechamentoResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PF_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar([FromQuery]FiltroPendenciasFechamentosDto filtro, [FromServices]IConsultasPendenciaFechamento consultasPendenciaFechamento)
        {
            return Ok(await consultasPendenciaFechamento.Listar(filtro));
        }

        [HttpGet("{pendenciaId}")]
        [ProducesResponseType(typeof(PendenciaFechamentoCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PF_C, Policy = "Bearer")]
        public async Task<IActionResult> Get(long pendenciaId, [FromServices]IConsultasPendenciaFechamento consultasPendenciaFechamento)
        {
            return Ok(await consultasPendenciaFechamento.ObterPorPendenciaId(pendenciaId));
        }

        [HttpPost("aprovar")]
        [ProducesResponseType(typeof(IEnumerable<AuditoriaPersistenciaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PF_A, Policy = "Bearer")]
        public async Task<IActionResult> Aprovar([FromBody] IEnumerable<long> pendenciasIds, [FromServices]IComandosPendenciaFechamento comandosPendenciaFechamento)
        {
            return Ok(await comandosPendenciaFechamento.Aprovar(pendenciasIds));
        }

        [HttpGet("{pendenciaId}/detalhamentos")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDetalhamentoPendenciasFechamento(long pendenciaId, [FromServices] IObterDetalhamentoPendenciaFechamentoConsolidadoUseCase useCase)
        {
            return Ok(await useCase.Executar(pendenciaId));
        }

        [HttpGet("{pendenciaId}/aulas/detalhamentos")]
        [ProducesResponseType(typeof(DetalhamentoPendenciaAulaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDetalhamentoPendenciasAula(long pendenciaId, [FromServices] IObterDetalhamentoPendenciaAulaUseCase useCase)
        {
            return Ok(await useCase.Executar(pendenciaId));
        }

    }
}
