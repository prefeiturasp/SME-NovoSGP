using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/diarios-bordo")]
    [Authorize("Bearer")]
    public class DiarioBordoController : ControllerBase
    {

        [HttpGet("{aulaId}")]
        [ProducesResponseType(typeof(DiarioBordoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter([FromServices] IObterDiarioBordoUseCase useCase, long aulaId)
        {
            var result = await useCase.Executar(aulaId);
            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromServices] IInserirDiarioBordoUseCase useCase, [FromBody] InserirDiarioBordoDto diarioBordoDto)
        {
            return Ok(await useCase.Executar(diarioBordoDto));
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromServices] IAlterarDiarioBordoUseCase useCase, [FromBody] AlterarDiarioBordoDto diarioBordoDto)
        {
            return Ok(await useCase.Executar(diarioBordoDto));
        }

        [HttpGet("devolutivas/{devolutivaId}")]
        [ProducesResponseType(typeof(DiarioBordoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorDevolutiva([FromServices] IObterDiariosBordoPorDevolutiva useCase, long devolutivaId)
        {
            return Ok(await useCase.Executar(devolutivaId));
        }

        [HttpGet("turmas/{turmaCodigo}/componentes-curriculares/{componenteCurricularId}/inicio/{dataInicio}/fim/{dataFim}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<DiarioBordoDevolutivaDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorIntervalo([FromServices] IObterDiariosDeBordoPorPeriodoUseCase useCase, string turmaCodigo, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            return Ok(await useCase.Executar(new FiltroTurmaComponentePeriodoDto(turmaCodigo, componenteCurricularId, dataInicio, dataFim)));
        }
    }
}
