using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
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
        public async Task<IActionResult> ObterFrequenciaGlobal(int anoLetivo, string codigoDre, string codigoUe, [FromServices] IConsultasRegistroFrequenciaAgrupamentoGlobalUseCase consultasRegistroFrequenciaAgrupamentoGlobalUseCase)
           => Ok(await consultasRegistroFrequenciaAgrupamentoGlobalUseCase.ObterFrequencia(anoLetivo, codigoDre, codigoUe));

        [HttpGet("frequencia-mensal")]
        [ProducesResponseType(typeof(PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaMensal(int anoLetivo, string codigoDre, string codigoUe, [FromServices] IConsultasRegistroFrequenciaAgrupamentoMensalUseCase consultasRegistroFrequenciaAgrupamentoMensalUseCase)
          => Ok(await consultasRegistroFrequenciaAgrupamentoMensalUseCase.ObterFrequencia(anoLetivo, codigoDre, codigoUe));

        [HttpGet("frequencia-ranking")]
        [ProducesResponseType(typeof(PainelEducacionalRegistroFrequenciaRankingDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaRanking(int anoLetivo, string codigoDre, string codigoUe, [FromServices] IConsultasRegistroFrequenciaAgrupamentoRankingUseCase consultasRegistroFrequenciaAgrupamentoRankingUseCase)
         => Ok(await consultasRegistroFrequenciaAgrupamentoRankingUseCase.ObterFrequencia(anoLetivo, codigoDre, codigoUe));

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

        [HttpGet("ideb")]
        [ProducesResponseType(typeof(PainelEducacionalIdebAgrupamentoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterIdeb([FromQuery] FiltroPainelEducacionalIdeb filtro, [FromServices] IConsultasIdebPainelEducacionalUseCase consultasIdebPainelEducacionalUseCase)
       => Ok(await consultasIdebPainelEducacionalUseCase.ObterIdeb(filtro));

        [HttpGet("visao-geral")]
        [ProducesResponseType(typeof(PainelEducacionalVisaoGeralRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterVisaoGeral(int anoLetivo, string codigoDre, string codigoUe, [FromServices] IConsultasVisaoGeralPainelEducacionalUseCase consultasVisaoGeralPainelEducacionalUseCase)
          => Ok(await consultasVisaoGeralPainelEducacionalUseCase.ObterVisaoGeralConsolidada(anoLetivo, codigoDre, codigoUe));

        [HttpGet("indicadores-pap")]
        [ProducesResponseType(typeof(PainelEducacionalInformacoesPapDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterIndicadoresPap([FromServices] IConsultasInformacoesPapUseCase consultasInformacoesPapUseCase, [FromQuery] FiltroPainelEducacionalDreUe filtro)
        => Ok(await consultasInformacoesPapUseCase.ObterInformacoesPap(filtro.CodigoDre, filtro.CodigoUe));

        [HttpGet("fluencia-leitora")]
        [ProducesResponseType(typeof(PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFluenciaLeitora([FromQuery] FiltroPainelEducacionalAnoLetivoPeriodo filtro, [FromServices] IConsultasPainelEducacionalFluenciaLeitoraUseCase consultasFluenciaLeitoraUseCase)
        => Ok(await consultasFluenciaLeitoraUseCase.ObterFluenciaLeitora(filtro.Periodo, filtro.AnoLetivo, filtro.CodigoDre));

        [HttpGet("taxa-alfabetizacao")]
        [ProducesResponseType(typeof(PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterConsolidacaoTaxaAlfabetizacao(int anoLetivo, string codigoDre, string codigoUe, [FromServices] IConsultasPainelEducacionalTaxaAlfabetizacaoUseCase consultasFluenciaLeitoraUseCase)
       => Ok(await consultasFluenciaLeitoraUseCase.Executar(anoLetivo, codigoDre, codigoUe));

        [HttpGet("abandono")]
        [ProducesResponseType(typeof(PainelEducacionalAbandonoModalidadeDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterConsolidacaoAbandono(int anoLetivo, string codigoDre, [FromServices] IConsultasAbandonoPainelEducacionalUseCase consultasAbandonoPainelEducacionalUseCase)
        => Ok(await consultasAbandonoPainelEducacionalUseCase.ObterAbandonoVisaoSmeDre(anoLetivo, codigoDre));

        [HttpGet("proficiencia-idep")]
        [ProducesResponseType(typeof(PainelEducacionalProficienciaIdepDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterProficienciaIdep(int anoLetivo, string codigoUe, [FromServices] IConsultasProficienciaIdebPainelEducacionalUseCase consultaProficienciaIdebPainelEducacionalUseCase)
         => Ok(await consultaProficienciaIdebPainelEducacionalUseCase.ObterProficienciaIdep(anoLetivo, codigoUe));

        [HttpGet("proficiencia-escolas-dados")]
        [ProducesResponseType(typeof(PainelEducacionalProficienciaEscolaDadosDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterProficienciaEscolaDados(string codigoUe, [FromServices] IConsultasProficienciaEscolaDadosUseCase consultaProficienciaEscolaDadosPainelEducacionalUseCase)
     => Ok(await consultaProficienciaEscolaDadosPainelEducacionalUseCase.ObterProficienciaEscolaDados(codigoUe));

        [HttpGet("sondagem-escrita")]
        [ProducesResponseType(typeof(PainelEducacionalSondagemEscritaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSondagemEscrita([FromQuery] FiltroPainelEducacionalAnoLetivoBimestre filtro, [FromServices] IConsultasSondagemEscritaUseCase consultasSondagemEscritaUseCase)
    => Ok(await consultasSondagemEscritaUseCase.ObterSondagemEscrita(filtro.CodigoDre, filtro.CodigoUe, filtro.AnoLetivo, filtro.Bimestre, filtro.SerieAno));

    }
}
