using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/unidades-escolares")]
    [Authorize("Bearer")]
    public class UnidadesEscolaresController : ControllerBase
    {
        [Route("{ueId}/funcionarios")]
        [HttpPost]
        public async Task<IActionResult> ObterFuncionariosPorUe([FromServices]IConsultasUnidadesEscolares consultasUnidadesEscolares, BuscaFuncionariosFiltroDto buscaFuncionariosFiltroDto)
        {
            return Ok(await consultasUnidadesEscolares.ObtemFuncionariosPorUe(ueId));
        }
    }
}