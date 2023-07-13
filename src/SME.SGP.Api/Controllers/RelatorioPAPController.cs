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
    [Route("api/v1/relatorios/pap")]
    [Authorize("Bearer")]
    public class RelatorioPAPController : ControllerBase
    {
        
        [HttpGet("periodos/{codigoTurma}")]
        [ProducesResponseType(typeof(IEnumerable<PeriodosPAPDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterPeriodos(string codigoTurma, [FromServices] IObterPeriodosPAPUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoTurma));
        }

        [HttpGet("turma/{codigoTurma}/aluno/{codigoAluno}/periodo/{periodoIdPAP}/secoes")]
        [ProducesResponseType(typeof(SecaoTurmaAlunoPAPDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterSecoes(string codigoTurma, string codigoAluno, long periodoIdPAP, [FromServices] IObterSecoesPAPUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroObterSecoesDto(codigoTurma, codigoAluno, periodoIdPAP)));
        }

        [HttpGet("turma/{codigoTurma}/aluno/{codigoAluno}/periodo/{periodoIdPAP}/questionario/{questionarioId}")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterQuestionario(string codigoTurma, string codigoAluno, long periodoIdPAP, long questionarioId, [FromQuery] long? papSecaoId, [FromServices] IObterQuestionarioPAPUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoTurma, codigoAluno, periodoIdPAP, questionarioId, papSecaoId));
        }
    }
}
