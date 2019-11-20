using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/grade/")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class GradeController : ControllerBase
    {
        [HttpGet("aulas/{turma}/{disciplina}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<GradeComponenteTurmaAulasDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterGradeAulasTurma(string turma, int disciplina, [FromServices] IConsultasGrade consultasGrade)
        {
            var horasGrade = await consultasGrade.ObterGradeAulasTurma(turma, disciplina);

            if (horasGrade != null)
                return Ok(horasGrade);
            else
                return StatusCode(204);
        }
    }
}