using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/periodo-escolar/integracoes")]
    [ChaveIntegracaoSgpApi]
    [ValidaDto]
    public class PeriodoEscolarIntegracaoController : ControllerBase
    {
        [HttpGet("periodo-atual")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(PeriodoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterPeriodoBimestreAtual(long turmaId, DateTime dataReferencia, [FromServices] IObterPeriodoEscolarAtualPorTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(turmaId, dataReferencia));
        }
    }
}
