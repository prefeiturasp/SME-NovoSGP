using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using SME.SGP.Infra.Dtos.PainelEducacional.Reclassificacao;
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
        public async Task<IActionResult> ObterIdepPorAnoEtapa(int anoLetivo, int etapa, string codigoDre, [FromServices] IConsultasIdepPainelEducacionalUseCase consultasIdepPainelEducacionalUseCase)
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
        [ProducesResponseType(typeof(IndicadoresPapDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterIndicadoresPap([FromServices] IConsultasInformacoesPapUseCase consultasInformacoesPapUseCase, [FromQuery] int anoLetivo, [FromQuery] string codigoDre, [FromQuery] string codigoUe)
        => Ok(await consultasInformacoesPapUseCase.ObterInformacoesPap(anoLetivo, codigoDre, codigoUe));

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

        [HttpGet("notas")]
        [ProducesResponseType(typeof(PainelEducacionalNotasVisaoSmeDreDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterNotasVisaoSmeDre([FromQuery] FiltroPainelEducacionalNotasVisaoSmeDre filtro, [FromServices] IConsultasNotasVisaoSmeDreUseCase consultasNotasUseCase)
     => Ok(await consultasNotasUseCase.ObterNotasVisaoSmeDre(filtro.CodigoDre, filtro.AnoLetivo, filtro.Bimestre, filtro.SerieAno));

        [HttpGet("notas-ue")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterNotasVisaoUe([FromQuery] FiltroPainelEducacionalNotasVisaoUe filtro, [FromServices] IConsultasNotasVisaoUeUseCase consultasNotasUseCase)
     => Ok(await consultasNotasUseCase.ObterNotasVisaoUe(filtro.CodigoUe, filtro.AnoLetivo, filtro.Bimestre, filtro.Modalidade));

        [HttpGet("reclassificacao")]
        [ProducesResponseType(typeof(PainelEducacionalReclassificacaoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterReclassificacao([FromQuery] FiltroPainelEducacionalReclassificacao filtro, [FromServices] IConsultasReclassificacaoPainelEducacionalUseCase consultasReclassificacaoUseCase)
     => Ok(await consultasReclassificacaoUseCase.ObterReclassificacao(filtro.CodigoDre, filtro.CodigoUe, filtro.AnoLetivo, filtro.AnoTurma));

    }
}
