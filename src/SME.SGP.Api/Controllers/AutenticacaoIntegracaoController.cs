using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/autenticacao/integracoes")]
    [ChaveIntegracaoSgpApi]
    public class AutenticacaoIntegracaoController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous]
        [Route("sga/frequencia")]
        public async Task<IActionResult> ObterGuidAutenticacaoFrequencia(SolicitacaoGuidAutenticacaoFrequenciaSGADto input, [FromServices] IObterGuidAutenticacaoFrequenciaSGA obterGuidAutenticacaoFrequenciaSGA)
        {
            var guidAutenticacaoFrequencia = await obterGuidAutenticacaoFrequenciaSGA.Executar(input);
            return Ok(guidAutenticacaoFrequencia);
        }
    }
    
}
