using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/unidades-escolares")]
    [Authorize("Bearer")]
    public class UnidadesEscolaresController : ControllerBase
    {
        [Route("{ueId}/funcionarios")]
        [HttpGet]
        public async Task<IActionResult> ObterFuncionariosPorUe([FromServices]IConsultasUnidadesEscolares consultasUnidadesEscolares, string ueId)
        {
            return Ok(await consultasUnidadesEscolares.ObtemFuncionariosPorUe(ueId));
        }
    }
}