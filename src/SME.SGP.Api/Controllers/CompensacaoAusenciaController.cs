using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/compensacoes/ausencia")]
    [Authorize("Bearer")]
    public class CompensacaoAusenciaController : ControllerBase
    {
        [HttpGet("turmas/{turmaCodigo}/bimestres/{bimestre}/aberto")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CA_C, Policy = "Bearer")]
        public async Task<IActionResult> VerificarPeriodoAberto(string turmaCodigo, int bimestre, [FromServices] IPeriodoDeCompensacaoAbertoUseCase useCase)
        {
            return Ok(await useCase.VerificarPeriodoAberto(turmaCodigo, bimestre));
        }

        [HttpGet()]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<CompensacaoAusenciaListagemDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CA_C, Policy = "Bearer")]
        public async Task<IActionResult> listar([FromQuery] FiltroCompensacoesAusenciaDto filtros, [FromServices] IConsultasCompensacaoAusencia consultas)
        {
            return Ok(await consultas.ListarPaginado(filtros.TurmaId, filtros.DisciplinaId, filtros.Bimestre, filtros.AtividadeNome, filtros.AlunoNome));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<CompensacaoAusenciaCompletoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CA_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter(long id, [FromServices] IConsultasCompensacaoAusencia consultas)
        {
            return Ok(await consultas.ObterPorId(id));
        }

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CA_I, Policy = "Bearer")]
        public async Task<IActionResult> Inserir([FromBody] CompensacaoAusenciaDto compensacao, [FromServices] IComandosCompensacaoAusencia comandos)
        {
            await comandos.Inserir(compensacao);
            return Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CA_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar(long id, [FromBody] CompensacaoAusenciaDto compensacao, [FromServices] IComandosCompensacaoAusencia comandos)
        {
            await comandos.Alterar(id, compensacao);
            return Ok();
        }

        [HttpDelete()]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CA_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir(long[] compensacoesIds, [FromServices] IComandosCompensacaoAusencia comandos)
        {
            await comandos.Excluir(compensacoesIds);
            return Ok();
        }

        [HttpGet("copiar/turmas/{turmaOrigemCodigo}")]
        [ProducesResponseType(typeof(IEnumerable<TurmaRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.CA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTurmasCopia(string turmaOrigemCodigo, [FromServices] IConsultasCompensacaoAusencia consultas)
        {
            return Ok(await consultas.ObterTurmasParaCopia(turmaOrigemCodigo));
        }

        [HttpPost("copiar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CA_I, Policy = "Bearer")]
        public async Task<IActionResult> Copiar([FromBody] CompensacaoAusenciaCopiaDto compensacaoCopia, [FromServices] IComandosCompensacaoAusencia comandos)
        {
            return Ok(await comandos.Copiar(compensacaoCopia));
        }

    }
}
