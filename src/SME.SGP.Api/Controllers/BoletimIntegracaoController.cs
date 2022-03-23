using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/boletim/integracoes")]
    [ApiController]
    [ChaveIntegracaoSgpApi]
    public class BoletimIntegracaoController : ControllerBase
    {
        [HttpPost("imprimir")]
        [ChaveIntegracaoSgpApi]
        public async Task<IActionResult> Imprimir([FromBody] FiltroRelatorioBoletimEscolaAquiDto filtroRelatorioBoletimDto, [FromServices] IBoletimEscolaAquiUseCase boletimUseCase)
        {
            return Ok(await boletimUseCase.Executar(filtroRelatorioBoletimDto));
        }
    }
}
