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
    [Route("api/v1/relatorios/pap")]
    [Authorize("Bearer")]
    public class RelatorioPAPController : ControllerBase
    {
        
        [HttpPost("periodos/{codigoTurma}")]
        [ProducesResponseType(typeof(IEnumerable<PeriodosPAPDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RAA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPeriodos(string codigoTurma, [FromServices] IObterPeriodosPAPUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoTurma));
        }
    }
}
