using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/exemplos/")]
    [Authorize("Bearer")]
    public class ExemploRelatorioController : ControllerBase
    {
        [HttpPost("games")]
        public async Task<IActionResult> ExemploGames([FromBody] FiltroRelatorioGamesDto filtroRelatorioGamesDto, [FromServices] IGamesUseCase gamesUseCase)
        {
            return Ok(await gamesUseCase.Executar(filtroRelatorioGamesDto));
        }
    }
}
