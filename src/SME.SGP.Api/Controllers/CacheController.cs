using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra.Utilitarios;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/cache")]
    public class CacheController : Controller
    {
        [HttpGet("chaves")]
        [ProducesResponseType(typeof(List<string>), 200)]
        [AllowAnonymous]
        public IActionResult ObterChaves()
        {
            return Ok(typeof(NomeChaveCache).ObterConstantesPublicas<string>());
        }

        [HttpGet("prefixos")]
        [ProducesResponseType(typeof(List<string>), 200)]
        [AllowAnonymous]
        public IActionResult ObterPrefixos([FromServices] IObterPrefixosCacheUseCase useCase)
        {
            return Ok(useCase.Executar());
        }
    }
}
