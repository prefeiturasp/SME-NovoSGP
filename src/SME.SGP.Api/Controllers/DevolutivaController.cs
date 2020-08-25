using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/devolutivas")]
    [Authorize("Bearer")]
    public class DevolutivaController : ControllerBase
    {

        [HttpGet("turmas/{turmaCodigo}/componentes-curriculares/{componenteCurricularCodigo}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<DevolutivaResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DE_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar(string turmaCodigo, long componenteCurricularCodigo, [FromQuery] DateTime? dataReferencia, [FromServices] IObterListaDevolutivasPorTurmaComponenteUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroListagemDevolutivaDto(turmaCodigo, componenteCurricularCodigo, dataReferencia)));
        }

        [HttpGet("{devolutivaId}")]
        [ProducesResponseType(typeof(DiarioBordoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorId(long devolutivaId, [FromServices] IObterDevolutivaPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(devolutivaId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.DE_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromServices] IInserirDevolutivaUseCase useCase, [FromBody] InserirDevolutivaDto devolutivaDto)
        {
            return Ok(await useCase.Executar(devolutivaDto));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DE_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar(long id, [FromServices] IAlterarDevolutivaUseCase useCase, [FromBody] AlterarDevolutivaDto devolutivaDto)
        {
            devolutivaDto.Id = id;
            return Ok(await useCase.Executar(devolutivaDto));
        }

        [HttpDelete("{devolutivaId}")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DE_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir(long devolutivaId, [FromServices] IExcluirDevolutivaUseCase useCase)
        {
            return Ok(await useCase.Executar(devolutivaId));
        }

        [HttpGet("turmas/{turmaCodigo}/componentes-curriculares/{componenteCurricularId}/sugestao")]
        [ProducesResponseType(typeof(DateTime), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DE_C, Policy = "Bearer")]
        public async Task<IActionResult> SugestaoDataInicio(string turmaCodigo, long componenteCurricularId, [FromServices] IObterUltimaDataDevolutivaPorTurmaComponenteUseCase useCase)
        {
            var data = await useCase.Executar(new FiltroTurmaComponenteDto(turmaCodigo, componenteCurricularId));

            if (data == DateTime.MinValue)
                return NoContent();

            return Ok(data.AddDays(1));
        }
    }

}
