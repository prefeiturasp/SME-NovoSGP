using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/dres/integracoes")]
    [ApiController]
    [ChaveIntegracaoSgpApi]
    public class DreIntegracaoController : ControllerBase
    {
        [HttpGet()]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTodasDres([FromServices] IObterDresUseCase useCase)
        {
            var dres = await useCase.Executar();
            return Ok(dres);
        }
    }
}