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
        public IActionResult Migrar(MigrarPlanejamentoAnualDto migrarPlanoAnualDto)
        {
            // TODO Incluir UseCase para copia do plano
            return Ok();
        }

        [HttpGet("turmas/copia")]
        [ProducesResponseType(typeof(IEnumerable<TurmaParaCopiaPlanoAnualDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTurmasParaCopia([FromQuery] int turmaId, [FromQuery] long componenteCurricular)
        {
            // TODO Incluir UseCase de consulta de Turmas para copia por componente e turma de origem

            return Ok(new List<TurmaParaCopiaPlanoAnualDto>()
            {
                new TurmaParaCopiaPlanoAnualDto()
                {
                    Id = 11,
                    Nome = "1A",
                    PossuiPlano = true,
                    TurmaId = 123123
                },
                new TurmaParaCopiaPlanoAnualDto()
                {
                    Id = 22,
                    Nome = "1B",
                    PossuiPlano = false,
                    TurmaId = 321321
                },
                new TurmaParaCopiaPlanoAnualDto()
                {
                    Id = 33,
                    Nome = "1C",
                    PossuiPlano = false,
                    TurmaId = 123321
                },
            });
        }

        [HttpGet("{planejamentoAnualId}/preiodos-escolares/copia")]
        [ProducesResponseType(typeof(IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterPeriodosEscolaresParaCopia(long planejamentoAnualId)
        {
            // TODO Incluir UseCase de consulta de Periodos Escolares com conteúdo para cópia

            return Ok(new List<PlanejamentoAnualPeriodoEscolarResumoDto>()
            {
                new PlanejamentoAnualPeriodoEscolarResumoDto()
                {
                    Id = 11,
                    Bimestre = 1
                },
                new PlanejamentoAnualPeriodoEscolarResumoDto()
                {
                    Id = 22,
                    Bimestre = 2
                },
                new PlanejamentoAnualPeriodoEscolarResumoDto()
                {
                    Id = 33,
                    Bimestre = 3
                },
            });
        }

    }
}