using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
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
        [HttpGet("aulas/turma-disciplina")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<GradeComponenteTurmaAulasDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterGradeAulasTurma([FromBody] FiltroGradeAulaTurmaDto filtro, [FromServices] IConsultasGrade consultasGrade)
        {
            var semana = (filtro.Data.DayOfYear / 7) + 1;
            var horasGrade = await consultasGrade.ObterGradeAulasTurma(filtro.Turma, filtro.Disciplina, semana.ToString());

            if (horasGrade != null)
                return Ok(horasGrade);
            else
                return StatusCode(204);
        }
    }
}