using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/turmas")]
    [Authorize("Bearer")]
    public class TurmaController : ControllerBase
    {
        [HttpGet("{turmaCodigo}/alunos/anos/{anoLetivo}")]
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
            return  Ok (await ObterTipoDeCalendarioDaTurmaUseCase.Executar(mediator, new ObterTipoDeCalendarioDaTurmaEntrada() { TurmaCodigo = turmaCodigo }));
        }
    }
}
