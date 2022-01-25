using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/mural/atividades/infantil")]
    [ValidaDto]
    public class AtividadeInfantilController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AtividadeInfantilDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_C, Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAtividadesMural([FromQuery] long aulaId, [FromServices] IObterAtividadesInfantilUseCase useCase)
        {
            return Ok(await useCase.Executar(aulaId));
        }
    }
}
