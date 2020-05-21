using MediatR;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/versoes")]
    public class GithubController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ObterUltimaVersao([FromServices]IMediator mediator)
        {
            return Ok(await ObterUltimaVersaoUseCase.Executar(mediator));
        }

    }
}