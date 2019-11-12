using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/grade/")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class GradeController : ControllerBase
    {
        [HttpGet("aulas/{turma}/{componente}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<GradeComponenteTurmaAulasDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterGradeAulasTurma(int turma, int componente)
        {
            if (turma == 1)
                return Ok(new GradeComponenteTurmaAulasDto() { QuantidadeAulasGrade = 5, QuantidadeAulasRestante = 2});
            else return StatusCode(204);
        }
    }
}