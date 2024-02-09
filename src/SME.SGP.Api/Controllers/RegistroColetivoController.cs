using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/registro-coletivo")]
    [Authorize("Bearer")]
    public class RegistroColetivoController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(TipoReuniaoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RC_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTipoDeReuniaoNAAPA([FromServices] IObterTiposDeReuniaoUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }
    }
}
