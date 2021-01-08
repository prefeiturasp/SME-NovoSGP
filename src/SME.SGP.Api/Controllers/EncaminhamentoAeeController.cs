using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System.Collections.Generic;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/encaminhamento-aee")]
    public class EncaminhamentoAeeController : ControllerBase
    {
        [HttpGet("secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSecoesPorEtapaDeEncaminhamentoAEE([FromQuery] long etapa, [FromServices] IObterSecoesPorEtapaDeEncaminhamentoAEEUseCase obterSecoesPorEtapaDeEncaminhamentoAEEUseCase)
        {
            return Ok(await obterSecoesPorEtapaDeEncaminhamentoAEEUseCase.Executar(etapa));
        }
        
        [HttpGet("questionario")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoAeeDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterQuestionario([FromQuery] long questionarioId, [FromQuery] long? encaminhamentoId, [FromServices] IObterQuestionarioEncaminhamentoAeeUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, encaminhamentoId));
        }
    }
}