using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/rabbit")]
    public class RabbitController : ControllerBase
    {
        [HttpGet]
        
        public async Task<IActionResult> Get([FromServices] ITrataDeadletterRabbitUseCase trataDeadletterRabbitUseCase)
        {
            return Ok(await trataDeadletterRabbitUseCase.Executar());
        }
    }
}
