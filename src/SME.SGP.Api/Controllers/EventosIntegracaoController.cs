using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/calendarios/eventos/integracoes")]
    [ApiController]
    [ChaveIntegracaoSgpApi]
    public class EventosIntegracaoController : ControllerBase
    {
        [HttpGet("liberacao-boletim/turmas/{turmaCodigo}/bimestres")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterBimestresLiberacaoBoletim(string turmaCodigo, [FromServices] IObterBimestresLiberacaoBoletimUseCase obterBimestresLiberacaoBoletimUseCase)
        {
            var retorno = await obterBimestresLiberacaoBoletimUseCase.Executar(turmaCodigo);
            return Ok(retorno);
        }
    }
}
