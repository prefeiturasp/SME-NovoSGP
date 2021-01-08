using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System.Collections.Generic;
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
    }
}
