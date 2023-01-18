using System;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/aula/integracoes/")]
    [ChaveIntegracaoSgpApi]
    [ValidaDto]
    public class AulaIntegracaoController : ControllerBase
    {
        [HttpGet("turma/{turmaCodigo}/componente-curricular/{componenteCurricular}/data/{dataAulaTicks}")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(IEnumerable<AulaQuantidadeTipoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAulasPorTurmaComponenteData(string turmaCodigo, long componenteCurricular, long dataAulaTicks,[FromServices] IObterAulasPorTurmaComponenteDataUseCase obterAulasPorTurmaComponenteDataUseCase)
        {
            return Ok(await obterAulasPorTurmaComponenteDataUseCase.Executar(new FiltroObterAulasPorTurmaComponenteDataDto(turmaCodigo, componenteCurricular, new DateTime(dataAulaTicks))));
        }
    }
}
