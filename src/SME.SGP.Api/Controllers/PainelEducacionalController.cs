using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaSemanalUe;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
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

        [HttpGet("frequencia-diaria-dre")]
        [ProducesResponseType(typeof(FrequenciaDiariaDreDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaDiariaDre([FromQuery] FiltroFrequenciaDiariaDreDto filtro, [FromServices] IConsultasRegistroFrequenciaDiariaDreUseCase consultasRegistroFrequenciaDiariaDreUseCase)
        => Ok(await consultasRegistroFrequenciaDiariaDreUseCase.ObterFrequenciaDiariaPorDre(filtro));

        [HttpGet("frequencia-diaria-ue")]
        [ProducesResponseType(typeof(FrequenciaDiariaUeDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaDiariaUe([FromQuery] FiltroFrequenciaDiariaUeDto filtro, [FromServices] IConsultasRegistroFrequenciaDiariaUeUseCase consultasRegistroFrequenciaDiariaUeUseCase)
        => Ok(await consultasRegistroFrequenciaDiariaUeUseCase.ObterFrequenciaDiariaPorUe(filtro));

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

        [HttpGet("abandono")]
        [ProducesResponseType(typeof(PainelEducacionalAbandonoModalidadeDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterConsolidacaoAbandono(int anoLetivo, string codigoDre, [FromServices] IConsultasAbandonoPainelEducacionalUseCase consultasAbandonoPainelEducacionalUseCase)
        => Ok(await consultasAbandonoPainelEducacionalUseCase.ObterAbandonoVisaoSmeDre(anoLetivo, codigoDre));

        [HttpGet("abandono-ue")]
        [ProducesResponseType(typeof(PainelEducacionalAbandonoModalidadeDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterConsolidacaoAbandonoUe(int anoLetivo, string codigoDre, string codigoUe, string modalidade, int numeroPagina, int numeroRegistros, [FromServices] IConsultasAbandonoPainelEducacionalUeUseCase consultasAbandonoPainelEducacionalUeUseCase)
        => Ok(await consultasAbandonoPainelEducacionalUeUseCase.Executar(anoLetivo, codigoDre, codigoUe, modalidade, numeroPagina, numeroRegistros));

        [HttpGet("proficiencia-idep")]
        [ProducesResponseType(typeof(PainelEducacionalProficienciaIdepDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterProficienciaIdep(int anoLetivo, string codigoUe, [FromServices] IConsultasProficienciaIdepPainelEducacionalUseCase consultaProficienciaIdepPainelEducacionalUseCase)
         => Ok(await consultaProficienciaIdepPainelEducacionalUseCase.ObterProficienciaIdep(anoLetivo, codigoUe));

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
        [ProducesResponseType(typeof(PaginacaoNotaResultadoDto<TurmaNotasVisaoUeDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterNotasVisaoUe([FromQuery] FiltroPainelEducacionalNotasVisaoUe filtro, [FromServices] IConsultasNotasVisaoUeUseCase consultasNotasUseCase)
            => Ok(await consultasNotasUseCase.ObterNotasVisaoUe(filtro.CodigoUe, filtro.AnoLetivo, filtro.Bimestre, filtro.Modalidade));

        [HttpGet("notas-ue/modalidades")]
        [ProducesResponseType(typeof(PaginacaoNotaResultadoDto<TurmaNotasVisaoUeDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterModalidadesNotasVisaoUe([FromQuery] int anoLetivo, string codigoUe, int bimestre, [FromServices] IConsultasModalidadesNotasVisaoUeUseCase consultasNotasUseCase)
          => Ok(await consultasNotasUseCase.ObterModalidadesNotasVisaoUe(anoLetivo, codigoUe, bimestre));

        [HttpGet("reclassificacao")]
        [ProducesResponseType(typeof(PainelEducacionalReclassificacaoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterReclassificacao([FromQuery] FiltroPainelEducacionalReclassificacao filtro, [FromServices] IConsultasReclassificacaoPainelEducacionalUseCase consultasReclassificacaoUseCase)
              => Ok(await consultasReclassificacaoUseCase.ObterReclassificacao(filtro.CodigoDre, filtro.CodigoUe, filtro.AnoLetivo, filtro.AnoTurma));

        [HttpGet("frequencia-semanal-ue")]
        [ProducesResponseType(typeof(PainelEducacionalFrequenciaSemanalUeDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaSemanalUe([FromQuery] FiltroFrequenciaSemanalUe filtro, [FromServices] IConsultasFrequenciaSemanalUeUseCase consultasFrequenciaSemanalUeCase)
             => Ok(await consultasFrequenciaSemanalUeCase.ObterFrequenciaSemanalUe(filtro.CodigoUe, filtro.AnoLetivo));

        [HttpGet("distorcao-serie-idade")]
        [ProducesResponseType(typeof(PainelEducacionalDistorcaoIdadeDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDistorcaoSerieIdade([FromQuery] FiltroPainelEducacionalDistorcaoIdade filtro, [FromServices] IConsultasDistorcaoIdadeUseCase consultasDistorcaoIdadeUseCase)
        => Ok(await consultasDistorcaoIdadeUseCase.ObterDistorcaoIdade(filtro));

        [HttpGet("proficiencia-ideb")]
        [ProducesResponseType(typeof(PainelEducacionalProficienciaIdebDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterProficienciaIdeb(int anoLetivo, string codigoUe, [FromServices] IConsultasProficienciaIdebPainelEducacionalUseCase consultaProficienciaIdebPainelEducacionalUseCase)
         => Ok(await consultaProficienciaIdebPainelEducacionalUseCase.ObterProficienciaIdeb(anoLetivo, codigoUe));

        [HttpGet("plano-aee")]
        [ProducesResponseType(typeof(PainelEducacionalPlanoAEEDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPlanosAEE([FromQuery] FiltroPainelEducacionalPlanosAEE filtro, [FromServices] IConsultasPlanosAEEPainelEducacionalUseCase consultasPlanosAEEPainelEducacionalUseCase)
         => Ok(await consultasPlanosAEEPainelEducacionalUseCase.ObterPlanosAEE(filtro));
    }
}
