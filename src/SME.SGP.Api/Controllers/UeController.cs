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
        public async Task<IActionResult> ObterModalidedes(string codigoUe, [FromServices]IConsultasUe consultasUe)
        {
            return Ok(await consultasUe.ObterModalidadesPorUe(codigoUe));
        }
    }
}