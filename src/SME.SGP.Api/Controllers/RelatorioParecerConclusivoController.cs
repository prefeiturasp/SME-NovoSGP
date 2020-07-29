using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/pareceres-conclusivos")]
    [Authorize("Bearer")]
    public class RelatorioParecerConclusivoController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Imprimir([FromBody] FiltroRelatorioParecerConclusivoDto filtroRelatorioParecerConclusivoDto, [FromServices]IImpressaoRelatorioParecerConclusivoUseCase impressaoUseCase)
        {
            return Ok(await impressaoUseCase.Executar(filtroRelatorioParecerConclusivoDto));
        }
    }
}
