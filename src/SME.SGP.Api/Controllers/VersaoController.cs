using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{

    //o frontend esta solicitando essa url na pagina de login
    //pelo que vi no blame passou a ser autorizada, esta certa essa modificacao? ou o front esta solicitando no momento errado
    //ou essa autorizacao nao deveria existir aqui
    [ApiController]
    [Route("api/v1/versoes")]
    [AllowAnonymous]
    public class VersaoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ObterUltimaVersao([FromServices]IObterUltimaVersaoUseCase obterUltimaVersaoUseCase)
        {
            return Ok(await obterUltimaVersaoUseCase.Executar());
        }

        [HttpGet("ping-test")]
        [AllowAnonymous]
        public async Task<IActionResult> TesteVelocidadePing()
        {
            return Ok();
        }
    }
}