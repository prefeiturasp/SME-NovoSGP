using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/boletim")]
    public class BoletimController : ControllerBase
    {
        [HttpGet("imprimir")]
        public async Task<IActionResult> Imprimir([FromQuery] FiltroRelatorioBoletimDto filtroRelatorioBoletimDto, [FromServices] IBoletimUseCase boletimUseCase)
        {
            return Ok(await boletimUseCase.Executar(filtroRelatorioBoletimDto));
        }
    }
}
