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
        [HttpGet("aulas/{turma}/{componente}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<GradeComponenteTurmaAulasDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterGradeAulasTurma(int turma, int componente, [FromServices] IConsultasGrade consultasGrade, [FromServices] IConsultasAbrangencia consultasAbrangencia)
        {
            var abrangencia = await consultasAbrangencia.ObterAbrangenciaTurma(turma);

            if (abrangencia == null)
                return StatusCode(601, "Abrangência da turma não localizada");

            var tipoEscola = abrangencia.TipoEscola;
            var modalidade = abrangencia.Modalidade;
            var duracao = abrangencia.QtDuracaoAula;

            var grade = await consultasGrade.ObterGradeTurma(tipoEscola, modalidade, duracao);
            if (grade != null)
                return Ok(grade);
            else
                return StatusCode(204);
        }
    }
}