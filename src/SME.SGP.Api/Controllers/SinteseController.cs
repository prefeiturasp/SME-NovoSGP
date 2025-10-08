using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/sinteses")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class SinteseController : ControllerBase
    {
        [HttpGet("{AnoLetivo}")]
        [ProducesResponseType(typeof(IEnumerable<SinteseDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ListarSintesesPorAnoLetivo([FromServices] IObterSintesePorAnoLetivoUseCase obterSintesePorAnoLetivoUseCase, int anoLetivo)
         => Ok(await obterSintesePorAnoLetivoUseCase.Executar(anoLetivo));
    }
}
