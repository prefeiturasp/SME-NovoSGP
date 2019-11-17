using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/frequencias")]
    [Authorize("Bearer")]
    public class FrequenciaController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.PDA_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar(long aulaId, [FromServices] IConsultasFrequencia consultasFrequencia)
        {
            return Ok(await consultasFrequencia.ObterListaFrequenciaPorAula(aulaId));
        }
    }
}