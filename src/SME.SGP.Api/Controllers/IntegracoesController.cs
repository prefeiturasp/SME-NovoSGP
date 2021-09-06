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
    [Route("api/v1/integracoes/")]
    [ChaveIntegracaoSgpApi]
    [ValidaDto]
    public class IntegracoesController : ControllerBase
    {
        [HttpGet("calendarios/ues/dias-letivos")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(IEnumerable<DiaLetivoSimplesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDiasLetivos([FromQuery] FiltroDiasLetivosPorUeEDataDTO filtro, [FromServices] IObterDiasLetivosPorUeETurnoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }
}
