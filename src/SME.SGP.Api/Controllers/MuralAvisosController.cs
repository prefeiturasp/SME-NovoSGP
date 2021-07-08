using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/mural/avisos")]
    [ValidaDto]
    public class MuralAvisosController : ControllerBase
    {
        private readonly IObterMuralAvisosUseCase _obterMuralAvisosUse;

        public MuralAvisosController(IObterMuralAvisosUseCase obterMuralAvisosUse)
        {
            _obterMuralAvisosUse = obterMuralAvisosUse;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MuralAvisosRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_C, Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromQuery] long aulaId)
        {
            return Ok(await _obterMuralAvisosUse.BuscarPorAulaId(aulaId));
        }
    }
}