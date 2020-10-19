using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ea/dashboard")]
    //[Authorize("Bearer")]
    public class DashboardEAController : ControllerBase
    {
        [HttpGet("adesao/usuarios/incompletos")]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 200)]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTotalUsuariosComAcessoIncompleto([FromQuery] string codigoDre, [FromQuery] long codigoUe, [FromServices] IObterTotalUsuariosComAcessoIncompletoUseCase obterUsuariosComAcessoIncompletoUseCase)
        {
            return Ok(await obterUsuariosComAcessoIncompletoUseCase.Executar(codigoDre, codigoUe));
        }

        [HttpGet("adesao/usuarios/validos")]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 200)]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTotalUsuariosValidos([FromQuery] string codigoDre, [FromQuery] long codigoUe, [FromServices] IObterTotalUsuariosComAcessoIncompletoUseCase obterUsuariosComAcessoIncompletoUseCase)
        {
            return Ok(await obterUsuariosComAcessoIncompletoUseCase.Executar(codigoDre, codigoUe));
        }
    }
}