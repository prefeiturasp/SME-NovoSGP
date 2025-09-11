using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Entidades;
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
        public async Task<IActionResult> ObterFrequenciaGlobal(string codigoDre, string codigoUe, [FromServices] IConsultasRegistroFrequenciaAgrupamentoGlobalUseCase consultasRegistroFrequenciaAgrupamentoGlobalUseCase)
           => Ok(await consultasRegistroFrequenciaAgrupamentoGlobalUseCase.ObterFrequencia(codigoDre, codigoUe));

        [HttpGet("frequencia-mensal")]
        [ProducesResponseType(typeof(PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaMensal(string codigoDre, string codigoUe, [FromServices] IConsultasRegistroFrequenciaAgrupamentoMensalUseCase consultasRegistroFrequenciaAgrupamentoMensalUseCase)
          => Ok(await consultasRegistroFrequenciaAgrupamentoMensalUseCase.ObterFrequencia(codigoDre, codigoUe));

        [HttpGet("frequencia-ranking")]
        [ProducesResponseType(typeof(PainelEducacionalRegistroFrequenciaRankingDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaRanking(string codigoDre, string codigoUe, [FromServices] IConsultasRegistroFrequenciaAgrupamentoRankingUseCase consultasRegistroFrequenciaAgrupamentoRankingUseCase)
         => Ok(await consultasRegistroFrequenciaAgrupamentoRankingUseCase.ObterFrequencia(codigoDre, codigoUe));

        [HttpGet("nivel-alfabetizacao")]
        [ProducesResponseType(typeof(PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDadosAlfabetizacao([FromQuery] FiltroPainelEducacionalAnoLetivoPeriodo filtro, [FromServices] IConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase consultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase)
         => Ok(await consultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase.ObterNumeroEstudantes(filtro.AnoLetivo, filtro.Periodo, filtro.CodigoDre, filtro.CodigoUe));

        [HttpGet("indicadores-alfabetizacao-critica")]
        [ProducesResponseType(typeof(PainelEducacionalIndicadorAlfabetizacaoCriticaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterIndicadoresAlfabetizacaoCritica([FromQuery] FiltroPainelEducacionalDreUe filtro, [FromServices] IConsultasAlfabetizacaoCriticaEscritaPainelEducacionalUseCase consultasIndicadoresNivelAlfabetizacaoCriticaUseCase)
         => Ok(await consultasIndicadoresNivelAlfabetizacaoCriticaUseCase.ObterNumeroEstudantes(filtro.CodigoDre, filtro.CodigoUe));

        [HttpGet("idep")]
        [ProducesResponseType(typeof(PainelEducacionalConsolidacaoIdep), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterIdepPorAnoEtapa(int anoLetivo, string etapa, string codigoDre, [FromServices] IConsultasIdepPainelEducacionalUseCase consultasIdepPainelEducacionalUseCase)
            => Ok(await consultasIdepPainelEducacionalUseCase.ObterIdepPorAnoEtapa(anoLetivo, etapa, codigoDre));

        [HttpGet("visao-geral")]
        [ProducesResponseType(typeof(PainelEducacionalVisaoGeralRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterVisaoGeral(int anoLetivo, string codigoDre, string codigoUe, [FromServices] IConsultasVisaoGeralPainelEducacionalUseCase consultasVisaoGeralPainelEducacionalUseCase)
          => Ok(await consultasVisaoGeralPainelEducacionalUseCase.ObterVisaoGeralConsolidada(anoLetivo, codigoDre, codigoUe));
    }
}
