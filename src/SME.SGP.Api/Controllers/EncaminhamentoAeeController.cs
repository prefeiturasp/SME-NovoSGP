using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("instrucoes-modal")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterInstrucoesModal([FromServices] IObterInstrucoesModalUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }
    }
}
