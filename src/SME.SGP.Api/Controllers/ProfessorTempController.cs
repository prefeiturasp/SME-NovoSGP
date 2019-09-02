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

        [HttpGet("{codigoRF}/escolas/{codigoEscola}/turmas/anos-letivos/{anoLetivo}")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Get(string codigoRF, string codigoEscola, int anoLetivo, [FromServices]IConsultasProfessor consultasProfessor)
        {
            return Ok(await consultasProfessor.ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(codigoRF, codigoEscola, anoLetivo));
        }
    }
}