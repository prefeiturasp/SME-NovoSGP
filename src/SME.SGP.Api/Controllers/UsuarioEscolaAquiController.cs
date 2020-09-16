using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/escola-aqui/usuarios")]
    //[Authorize("Bearer")]
    public class UsuarioEscolaAquiController : ControllerBase
    {
        [HttpPut("reiniciar-senha")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Authorize(Policy = "Bearer")]
        public async Task<IActionResult> ReiniciarSenha([FromBody] SolicitarReiniciarSenhaEscolaAquiDto solicitarReiniciarSenhaDto, [FromServices] ISolicitarReiniciarSenhaEscolaAquiUseCase solicitarReiniciarSenhaEscolaAquiUseCase)
        {
            return Ok(await solicitarReiniciarSenhaEscolaAquiUseCase.Executar(solicitarReiniciarSenhaDto.Cpf));
        }

        [HttpGet("dre/{codigoDre}/ue/{codigoUe}/cpf/{cpf}")]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 200)]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Authorize(Policy = "Bearer")]
        public async Task<IActionResult> ObterUsuarioPorCpf(string codigoDre, long codigoUe, string cpf, [FromServices] IObterUsuarioPorCpfUseCase obterUsuarioPorCpfUseCase)
        {
            return Ok(await obterUsuarioPorCpfUseCase.Executar(codigoDre, codigoUe, cpf));
        }
    }
}