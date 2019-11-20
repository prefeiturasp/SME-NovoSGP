using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    public class FrequenciaController : ControllerBase
    {
        [HttpGet]
        [Route("api/v1/calendarios/frequencias")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [Permissao(Permissao.PDA_I, Policy = "Bearer")]
        public async Task<IActionResult> Listar(long aulaId, [FromServices] IConsultasFrequencia consultasFrequencia)
        {
            return Ok(await consultasFrequencia.ObterListaFrequenciaPorAula(aulaId));
        }

        [HttpGet]
        [Route("api/v1/calendarios/{calendarioId}/frequencias/aulas/datas/turmas/{turmaId}/disciplinas/{disciplinaId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        //[Permissao(Permissao.PDA_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(long calendarioId, string turmaId, string disciplinaId, [FromServices] IConsultasAula consultasAula)
        {
            return Ok(await consultasAula.ObterDatasDeAulasPorCalendarioTurmaEDisciplina(calendarioId, turmaId, disciplinaId));
        }
    }
}