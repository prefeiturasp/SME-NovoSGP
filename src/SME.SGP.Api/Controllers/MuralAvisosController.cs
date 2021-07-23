using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/mural/avisos")]
    [ValidaDto]
    public class MuralAvisosController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(MuralAvisosRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_C, Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromQuery] long aulaId, [FromServices] IObterMuralAvisosUseCase useCase)
        {
            return Ok(await useCase.BuscarPorAulaId(aulaId));
        }

        [HttpPut("{avisoId}")]
        [ProducesResponseType(typeof(MuralAvisosRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.PDA_A, Permissao.DDB_A, Policy = "Bearer")]
        public async Task<IActionResult> Post(long avisoId, [FromBody] MensagemAvisoDto dto, [FromServices] IAlterarAvisoMuralUseCase useCase)
        {
            await useCase.Executar(avisoId, dto.Mensagem);
            return Ok();
        }
    }
}