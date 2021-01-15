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
    [Route("api/v1/planejamento/anual")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class PlanejamentoAnualController : ControllerBase
    {
        [HttpPost("turmas/{turmaId}/componentes-curriculares/{componenteCurricularId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar(long turmaId, long componenteCurricularId, [FromBody] SalvarPlanejamentoAnualDto dto, [FromServices] ISalvarPlanejamentoAnualUseCase useCase)
        {
            return Ok(await useCase.Executar(turmaId, componenteCurricularId, dto));
        }

        [HttpGet("turmas/{turmaId}/componentes-curriculares/{componenteCurricularId}/periodos-escolares/{periodoEscolarId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(PlanejamentoAnualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter(long turmaId, long componenteCurricularId,long periodoEscolarId, [FromServices] IObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCase useCase)
        {
            return Ok(await useCase.Executar(turmaId, componenteCurricularId, periodoEscolarId));
        }

        [HttpGet("turmas/{turmaId}/componentes-curriculares/{componenteCurricularId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPlanejamentoId(long turmaId, long componenteCurricularId, [FromServices] IObterPlanejamentoAnualPorTurmaComponenteUseCase useCase)
        {
            return Ok(await useCase.Executar(turmaId, componenteCurricularId));
        }


        [HttpPost("migrar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Permissao.PA_A, Policy = "Bearer")]
        public async Task<IActionResult> Migrar(MigrarPlanejamentoAnualDto migrarPlanoAnualDto, [FromServices] IMigrarPlanejamentoAnualUseCase useCase)
        {
            return Ok(await useCase.Executar(migrarPlanoAnualDto));
        }

        [HttpGet("turmas/copia")]
        [ProducesResponseType(typeof(IEnumerable<TurmaParaCopiaPlanoAnualDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTurmasParaCopia([FromQuery] int turmaId, [FromQuery] long componenteCurricular, [FromQuery] bool ensinoEspecial, [FromQuery] bool consideraHistorico, [FromServices] IObterTurmasParaCopiaUseCase useCase)
        {
            return Ok(await useCase.Executar(turmaId, componenteCurricular, ensinoEspecial, consideraHistorico ));
        }

        [HttpGet("{planejamentoAnualId}/periodos-escolares/copia")]
        [ProducesResponseType(typeof(IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterPeriodosEscolaresParaCopia(long planejamentoAnualId, [FromServices] IObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase useCase)
        {
            return Ok(await useCase.Executar(planejamentoAnualId));
        }

    }
}