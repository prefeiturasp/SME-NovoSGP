using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicao/esporadica")]
    [ValidaDto]
    public class AtribuicaoEsporadicaController : ControllerBase
    {
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_E, Permissao.AE_I, Policy = "Bearer")]
        public async Task<IActionResult> Excluir([FromServices]IComandosAtribuicaoEsporadica comandosAtribuicaoEsporadica, long id)
        {
            await comandosAtribuicaoEsporadica.Excluir(id);
            return Ok();
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AtribuicaoEsporadicaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar([FromQuery]FiltroAtribuicaoEsporadicaDto filtro, [FromServices]IConsultasAtribuicaoEsporadica consultasAtribuicaoEsporadica)
        {
            return Ok(await consultasAtribuicaoEsporadica.Listar(filtro));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AtribuicaoEsporadicaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_C, Policy = "Bearer")]
        public IActionResult Obter(long id, [FromServices]IConsultasAtribuicaoEsporadica consultasAtribuicaoEsporadica)
        {
            var atribuicaoEsporadica = consultasAtribuicaoEsporadica.ObterPorId(id);
            if (atribuicaoEsporadica is null)
                return StatusCode(204);
            else return Ok(atribuicaoEsporadica);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_A, Permissao.AE_I, Policy = "Bearer")]
        public IActionResult Post([FromBody]AtribuicaoEsporadicaCompletaDto atribuicaoEsporadicaCompletaDto, [FromServices]IComandosAtribuicaoEsporadica comandosAtribuicaoEsporadica)
        {
            comandosAtribuicaoEsporadica.Salvar(atribuicaoEsporadicaCompletaDto);

            return Ok();
        }
    }
}