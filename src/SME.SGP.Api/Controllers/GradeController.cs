using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/grades/")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class GradeController : ControllerBase
    {
        [HttpGet("aulas/turmas/{codigoTurma}/disciplinas/{codigoDisciplina}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<GradeComponenteTurmaAulasDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterGradeAulasTurma([FromQuery] DateTime data, string codigoTurma, long codigoDisciplina, [FromServices] IConsultasGrade consultasGrade, [FromQuery]bool ehRegencia = false)
        {
            var horasGrade = await consultasGrade.ObterGradeAulasTurmaProfessor(codigoTurma, codigoDisciplina, UtilData.ObterSemanaDoAno(data), data, ehRegencia: ehRegencia);

            if (horasGrade == null)
                return NoContent();

            return Ok(horasGrade);
        }
    }
}