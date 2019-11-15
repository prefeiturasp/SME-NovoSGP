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
        public async Task<IActionResult> ObterGradeAulasTurma(int turma, int disciplina, [FromServices] IConsultasGrade consultasGrade, [FromServices] IConsultasAbrangencia consultasAbrangencia, [FromServices] IConsultasAula consultasAula)
        {
            // Busca abrangencia a partir da turma
            var abrangencia = await consultasAbrangencia.ObterAbrangenciaTurma(turma);
            if (abrangencia == null)
                return StatusCode(601, "Abrangência da turma não localizada.");

            // Busca grade a partir dos dados da abrangencia da turma
            var grade = await consultasGrade.ObterGradeTurma(abrangencia.TipoEscola, abrangencia.Modalidade, abrangencia.QtDuracaoAula);
            if (grade == null)
                return StatusCode(601, "Grade da turma não localizada.");

            // Busca carga horaria na grade da disciplina para o ano da turma
            var horasGrade = await consultasGrade.ObterHorasGradeComponente(grade.Id, disciplina, abrangencia.Ano);
            // Busca horas aula cadastradas para a disciplina na turma
            var horascadastradas = await consultasAula.ObterQuantidadeAulasTurma(turma.ToString(), disciplina.ToString());

            if (horasGrade > 0)
                return Ok(new GradeComponenteTurmaAulasDto 
                { 
                    QuantidadeAulasGrade = horasGrade, 
                    QuantidadeAulasRestante = horasGrade - horascadastradas
                });
            else
                return StatusCode(204);
        }
    }
}