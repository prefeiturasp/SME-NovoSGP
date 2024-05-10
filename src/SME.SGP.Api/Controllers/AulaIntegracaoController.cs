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
        /// <summary>
        /// Retorno das aulas cadastradas no dia de acordo com componente e turma.
        /// </summary>
        /// <response code="200">A consulta foi realizada com sucesso.</response>
        /// <response code="500">Ocorreu um erro inesperado durante a consulta.</response>
        /// <response code="601">Houve uma falha de validação durante a consulta.</response>
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
