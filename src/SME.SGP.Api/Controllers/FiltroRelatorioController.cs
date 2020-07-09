using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/filtros")]
    [Authorize("Bearer")]
    public class FiltroRelatorioController : ControllerBase
    {
        [HttpGet("dres")]
        public async Task<IActionResult> ObterDresPorAbrangencia([FromServices] IObterFiltroRelatoriosDresPorAbrangenciaUseCase obterDresPorAbrangenciaFiltroRelatoriosUseCase)
        {
            return Ok(await obterDresPorAbrangenciaFiltroRelatoriosUseCase.Executar());
        }

        [HttpGet("ues/{codigoDre}")]
        public async Task<IActionResult> ObterUesPorAbrangencia(string codigoDre, [FromServices] IObterFiltroRelatoriosUesPorAbrangenciaUseCase obterFiltroRelatoriosUesPorAbrangenciaUseCase)
        {
            return Ok(await obterFiltroRelatoriosUesPorAbrangenciaUseCase.Executar(codigoDre));
        }
    }
}