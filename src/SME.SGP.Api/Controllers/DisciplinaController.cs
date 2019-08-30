using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/professores/")]
    [ValidaDto]
    public class ProfessorTempController : ControllerBase
    {
        [HttpGet("{codigoRF}/turmas/{codigoTurma}/disciplinas/")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Get(long codigoTurma, string codigoRF, [FromServices]IConsultasDisciplina consultasDisciplina)
        {
            return Ok(await consultasDisciplina.ObterDisciplinasPorProfessorETurma(codigoTurma, codigoRF));
        }
    }
}