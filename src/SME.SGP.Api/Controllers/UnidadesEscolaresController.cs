using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/unidades-escolares")]
    [Authorize("Bearer")]
    public class UnidadesEscolaresController : ControllerBase
    {
        [Route("funcionarios")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AS_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFuncionarios([FromServices] IObterFuncionariosUseCase obterFuncionariosUseCase,
                                                                          [FromBody]FiltroFuncionarioDto filtroFuncionariosDto)
        {
            return Ok(await obterFuncionariosUseCase.Executar(filtroFuncionariosDto));
        }

        [Route("usuarios")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.US_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterUsuarios([FromServices] IObterUsuarioFuncionarioUseCase obterUsuarioUseCase,
                                                       [FromBody] FiltroFuncionarioDto filtroFuncionariosDto)
        {
            return Ok(await obterUsuarioUseCase.Executar(filtroFuncionariosDto));
        }
    }
}