using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/cache")]
    public class CacheController : Controller
    {
        [HttpGet("chaves")]
        [AllowAnonymous]
        public IActionResult ObterChaves()
        {
            return Ok(typeof(NomeChaveCache).ObterConstantesPublicas<string>());
        }
    }
}
