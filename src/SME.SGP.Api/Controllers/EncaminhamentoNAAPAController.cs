using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/encaminhamento-naapa")]
    [Authorize("Bearer")]
    public class EncaminhamentoNAAPAController : ControllerBase
    {

        [HttpGet("secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesPorEtapaDeEncaminhamentoNAAPA([FromQuery] long encaminhamentoAeeId, [FromServices] IObterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase obterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase)
        {
            return Ok(await obterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase.Executar(encaminhamentoAeeId));
        }

        [HttpGet("questionario")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionario([FromQuery] long questionarioId, [FromQuery] long? encaminhamentoId, [FromQuery] string codigoAluno, [FromQuery] string codigoTurma, [FromServices] IObterQuestionarioEncaminhamentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, encaminhamentoId, codigoAluno, codigoTurma));
        }
    }
}
