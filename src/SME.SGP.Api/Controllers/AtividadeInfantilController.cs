using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/mural/atividade-infantil")]
    [ValidaDto]
    public class AtividadeInfantilController : ControllerBase
    {
        [HttpGet("obter-lista-atividades-mural")]
        [ProducesResponseType(typeof(AtividadeInfantilDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_C, Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterListaAtividadesMural([FromQuery] long aulaId, [FromServices] IObterListaAtividadeMuralUseCase useCase)
        {
            return Ok(await useCase.BuscarPorAulaId(aulaId));
        }
    }
}
