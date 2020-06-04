using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/exemplos/")]

    public class ExemploRelatorioController : ControllerBase
    {

        [HttpGet("games/anos/{ano}")]
        public async Task<IActionResult> ExemploGames(int ano, [FromServices] IGamesUseCase gamesUseCase)
        {
            await gamesUseCase.Executar();
            return Ok("");
        }
    }
}
