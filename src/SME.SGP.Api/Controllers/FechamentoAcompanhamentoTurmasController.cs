using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api
{
    [ApiController]
    [Route("api/v1/fechamentos/acompanhamentos")]
    [Authorize("Bearer")]
    public class FechamentoAcompanhamentoTurmasController : ControllerBase
    {
        [HttpGet("turmas")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF, Policy = "Bearer")]
        public async Task<IActionResult> ListaTurmas([FromQuery] FiltroAcompanhamentoFechamentoTurmasDto filtro, [FromServices] IObterTurmasFechamentoAcompanhamentoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("turmas/{turmaId}/fechamentos/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(IEnumerable<StatusTotalFechamentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF, Policy = "Bearer")]
        public async Task<IActionResult> ListaTotalStatusFechamentos(long turmaId, int bimestre, [FromServices] IObterFechamentoConsolidadoPorTurmaBimestreUseCase useCase)
        {
            var listaStatus = await useCase.Executar(new FiltroFechamentoConsolidadoTurmaBimestreDto(turmaId, bimestre));

            return Ok(listaStatus);
        }
        [HttpGet("turmas/{turmaId}/conselho-classe/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(IEnumerable<StatusTotalFechamentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF, Policy = "Bearer")]
        public async Task<IActionResult> ListaTotalStatusConselhosClasse(long turmaId, int bimestre, [FromServices] IObterConselhoClasseConsolidadoPorTurmaBimestreUseCase useCase)
        {
            var listaStatus = await useCase.Executar(new FiltroConselhoClasseConsolidadoTurmaBimestreDto(turmaId, bimestre));

            return Ok(listaStatus);
        }
    }
}
