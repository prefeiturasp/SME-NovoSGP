using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [ValidaDto]
    [Route("api/v1/cep")]
    public class CEPController : ControllerBase
    {
        [HttpGet("{cep}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(bool), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous]
        public async Task<IActionResult> BuscarCep([FromRoute] string cep, [FromServices] IBuscaCepUseCase buscaCepUseCase)
        {
            var retorno = await buscaCepUseCase.Executar(cep);
            
            if (retorno.EhNulo())
                return NoContent();
            
            return Ok(retorno);
        }
    }
}