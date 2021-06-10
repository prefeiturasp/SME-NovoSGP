using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
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

        [HttpGet("turmas/{turmaCodigo}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<DevolutivaResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DE_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar(string turmaCodigo, [FromQuery] long componenteCurricularCodigo, [FromQuery] DateTime? dataReferencia, [FromServices] IObterListaDevolutivasPorTurmaComponenteUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroListagemDevolutivaDto(turmaCodigo, componenteCurricularCodigo, dataReferencia)));
        }

        [HttpGet("{devolutivaId}")]
        [ProducesResponseType(typeof(DevolutivaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorId(long devolutivaId, [FromServices] IObterDevolutivaPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(devolutivaId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DE_I, Policy = "Bearer")]
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
        public async Task<IActionResult> SugestaoDataInicio(string turmaCodigo, long componenteCurricularId, [FromServices] IObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase useCase)
        {
           var data = await useCase.Executar(new FiltroTurmaComponenteDto(turmaCodigo, componenteCurricularId));

            if (!data.HasValue)
                return NoContent();

            return Ok(data.Value);
        }


        [HttpGet("periodo-dias")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPeriodoDeDiasDevolutiva(TipoParametroSistema tipo, int anoLetivo, [FromServices] IObterPeriodoDeDiasDevolutivaUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroTipoParametroAnoDto(tipo, anoLegivo)));
        }
    }

}
