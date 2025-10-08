using Microsoft.AspNetCore.Authorization;
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
    [Authorize("Bearer")]
    public class AtribuicaoEsporadicaController : ControllerBase
    {
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_E, Permissao.AE_I, Policy = "Bearer")]
        public async Task<IActionResult> Excluir([FromServices] IExcluirAtribuicaoEsporadicaUseCase useCase, long id)
        {
            return Ok(await useCase.Executar(id));
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AtribuicaoEsporadicaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar([FromQuery] FiltroAtribuicaoEsporadicaDto filtro, [FromServices] IListarAtribuicaoEsporadicaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AtribuicaoEsporadicaCompletaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter(long id, [FromServices] IConsultasAtribuicaoEsporadica consultasAtribuicaoEsporadica)
        {
            var atribuicaoEsporadica = await consultasAtribuicaoEsporadica.ObterPorId(id);

            if (atribuicaoEsporadica is null)
                return NoContent();

            return Ok(atribuicaoEsporadica);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_A, Permissao.AE_I, Policy = "Bearer")]
        public async Task<IActionResult> Post([FromBody] AtribuicaoEsporadicaDto atribuicaoEsporadicaDto, [FromServices] IComandosAtribuicaoEsporadica comandosAtribuicaoEsporadica)
        {
            await comandosAtribuicaoEsporadica.Salvar(atribuicaoEsporadicaDto);

            return Ok();
        }

        [HttpGet("periodos/ues/{ueId}/anos/{AnoLetivo}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AtribuicaoEsporadicaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPeriodoAtribuicaoPorUe(long ueId, int anoLetivo, [FromServices] IObterPeriodoAtribuicaoPorUeUseCase useCase)
        {
            return Ok(await useCase.Executar(ueId, anoLetivo));
        }
    }
}
