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
    [Route("api/v1/calendario/integracoes/")]
    [ChaveIntegracaoSgpApi]
    [ValidaDto]
    public class CalendarioIntegracoesController : ControllerBase
    {
        [HttpGet("ues/dias-letivos")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(IEnumerable<DiaLetivoSimplesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDiasLetivos([FromQuery] FiltroDiasLetivosPorUeEDataDTO filtro, [FromServices] IObterDiasLetivosPorUeETurnoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }
}
