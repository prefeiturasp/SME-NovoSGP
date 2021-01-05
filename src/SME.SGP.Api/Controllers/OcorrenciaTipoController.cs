using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ocorrencias/tipos")]
    //[Authorize("Bearer")]
    public class OcorrenciaTipoController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OcorrenciaTipoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        // O permissionamento será adicionado em uma task separada
        public async Task<IActionResult> Get([FromServices] IListarTiposOcorrenciaUseCase useCase)
        {
            var result = await useCase.Executar();
            if (result == null || !result.Any())
                return NoContent();

            return Ok(result);
        }
    }
}