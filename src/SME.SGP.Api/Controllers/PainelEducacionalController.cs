using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/painel-educacional")]
    [Authorize("Bearer")]
    public class PainelEducacionalController : ControllerBase
    {
        [HttpGet("frequencia-global")]
        [ProducesResponseType(typeof(PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaGlobal([FromServices] IConsultasRegistroFrequenciaAgrupamentoGlobalUseCase consultasRegistroFrequenciaAgrupamentoGlobalUseCase)
           => Ok(await consultasRegistroFrequenciaAgrupamentoGlobalUseCase.ObterFrequencia());

        [HttpGet("frequencia-mensal")]
        [ProducesResponseType(typeof(PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaMensal([FromServices] IConsultasRegistroFrequenciaAgrupamentoMensalUseCase consultasRegistroFrequenciaAgrupamentoMensalUseCase)
          => Ok(await consultasRegistroFrequenciaAgrupamentoMensalUseCase.ObterFrequencia());

        [HttpGet("frequencia-ranking")]
        [ProducesResponseType(typeof(PainelEducacionalRegistroFrequenciaRankingDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaRanking(string codigoDre, string codigoUe, [FromServices] IConsultasRegistroFrequenciaAgrupamentoRankingUseCase consultasRegistroFrequenciaAgrupamentoRankingUseCase)
         => Ok(await consultasRegistroFrequenciaAgrupamentoRankingUseCase.ObterFrequencia(codigoDre, codigoUe));
    }
}
