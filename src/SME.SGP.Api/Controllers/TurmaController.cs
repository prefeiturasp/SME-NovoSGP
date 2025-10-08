using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/turmas")]
    [Authorize("Bearer")]
    public class TurmaController : ControllerBase
    {
        [HttpGet("{turmaCodigo}/alunos/anos/{AnoLetivo}")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDadosBasicosDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterAlunosTurma(string turmaCodigo, int anoLetivo, [FromServices] IConsultasTurma consultas)
           => Ok(await consultas.ObterDadosAlunos(turmaCodigo, anoLetivo));

        [HttpGet("{turmaCodigo}/tipo-calendario")]
        [ProducesResponseType(typeof(IEnumerable<TipoCalendarioSugestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterSugestaoTipoCalendario(string turmaCodigo, [FromServices] IMediator mediator)
        {
            return Ok(await ObterTipoDeCalendarioDaTurmaUseCase.Executar(mediator, new ObterTipoDeCalendarioDaTurmaEntrada() { TurmaCodigo = turmaCodigo }));
        }
        [HttpGet("modalidades")]
        [ProducesResponseType(typeof(IEnumerable<TurmaModalidadeCodigoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterModalidades([FromQuery] string[] turmasCodigo, [FromServices] IObterTurmaModalidadesPorCodigosUseCase obterTurmaModalidadesPorCodigos)
        {
            return Ok(await obterTurmaModalidadesPorCodigos.Executar(turmasCodigo));
        }

        [HttpGet("listagem-turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Listar([FromQuery] FiltroTurmaDto filtroTurmaDto, [FromServices] IListarTurmasComComponentesUseCase listarTurmasComComponentesUseCase)
        {
            var retorno = await listarTurmasComComponentesUseCase.Executar(filtroTurmaDto);

            if (retorno.EhNulo())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("ues/{ueCodigo}/sondagem")]
        [ProducesResponseType(typeof(IEnumerable<TurmaRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTurmasSondagem(string ueCodigo, [FromQuery] int anoLetivo, [FromServices] IObterTurmaSondagemUseCase useCase)
        {
            return Ok(await useCase.Executar(ueCodigo, anoLetivo));
        }
    }
}
