using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ues")]
    [Authorize("Bearer")]
    public class UeController : ControllerBase
    {
        [HttpGet("{codigoUe}/modalidades")]
        [ProducesResponseType(typeof(IEnumerable<ModalidadeRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterModalidedes(string codigoUe, [FromQuery]int ano, [FromServices]IConsultasUe consultasUe)
        {
            return Ok(await consultasUe.ObterModalidadesPorUe(codigoUe, ano));
        }

        [HttpGet("{codigoUe}/modalidades/{idModalidade}")]
        [ProducesResponseType(typeof(IEnumerable<TurmaRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTurmas(string codigoUe, int idModalidade, [FromQuery]int ano, [FromServices]IConsultasUe consultasUe)
        {
            return Ok(await consultasUe.ObterTurmas(codigoUe, idModalidade, ano));
        }

        [HttpGet("/api/v1/dres/{codigoDre}/ues/professores/{professorRf}/atribuicoes")]
        [ProducesResponseType(typeof(IEnumerable<TurmaRetornoDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterUesAtribuicoes(string professorRf, string codigoDre, [FromServices] IConsultasAtribuicoes consultasAtribuicoes)
        {
            IEnumerable<TurmaRetornoDto> ues = await consultasAtribuicoes.ObterUes(professorRf, codigoDre);

            return Ok(ues);
        }
    }
}