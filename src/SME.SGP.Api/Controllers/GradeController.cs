using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

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
        [ProducesResponseType(typeof(GradeComponenteTurmaAulasDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterGradeAulasTurma([FromQuery] DateTime data, string codigoTurma, long codigoDisciplina, [FromServices] IMediator mediator, [FromQuery] bool ehRegencia = false)
        {
            var horasGrade = await ObterGradeAulasPorTurmaEProfessorUseCase.Executar(mediator, codigoTurma, codigoDisciplina, data, ehRegencia: ehRegencia);

            if (horasGrade == null)
                return NoContent();

            return Ok(horasGrade);
        }
    }
}