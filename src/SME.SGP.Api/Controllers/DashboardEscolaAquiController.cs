using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
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

        [HttpGet("adesao")]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 200)]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTotaisAdesao([FromQuery] string codigoDre, [FromQuery] long codigoUe, [FromServices] IObterTotaisAdesaoUseCase obterTotaisAdesaoUseCase)
        {
            return Ok(await obterTotaisAdesaoUseCase.Executar(codigoDre, codigoUe));
        }

        [HttpGet("ultimoProcessamento")]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 200)]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterUltimaAtualizacaoPorProcesso([FromQuery] string nomeProcesso, [FromServices] IObterUltimaAtualizacaoPorProcessoUseCase obterUltimaAtualizacaoPorProcessoUseCase)
        {
            return Ok(await obterUltimaAtualizacaoPorProcessoUseCase.Executar(nomeProcesso));
        }
    }
}