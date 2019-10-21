using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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
        public async Task<IActionResult> ObterFuncionariosPorUe([FromServices]IConsultasUnidadesEscolares consultasUnidadesEscolares,
            BuscaFuncionariosFiltroDto buscaFuncionariosFiltroDto, string ueId)
        {
            if (string.IsNullOrEmpty(ueId))
                throw new NegocioException("É necessário informar o código da UE.");
            buscaFuncionariosFiltroDto.AtualizaCodigoUe(ueId);
            return Ok(await consultasUnidadesEscolares.ObtemFuncionariosPorUe(buscaFuncionariosFiltroDto));
        }
    }
}