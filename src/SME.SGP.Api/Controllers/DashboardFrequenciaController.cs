using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v1/dashboard/frequencias")]
    public class DashboardFrequenciaController : Controller
    {
        [HttpGet("consolidacao")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(DateTime), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 204)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> UltimaConsolidacao(int anoLetivo, [FromServices] IObterDataConsolidacaoFrequenciaUseCase useCase)
        {
            var ultimaConsolidacao = await useCase.Executar(anoLetivo);

            if (!ultimaConsolidacao.HasValue)
                return NoContent();

            return Ok(ultimaConsolidacao.Value);
        }


        [HttpGet("modalidades/ano")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> ModalidadesPorAno([FromQuery] int anoLetivo, long dreId, long ueId, int modalidade, int semestre, [FromServices] IObterModalidadesAnoUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade, semestre));
        }

        [HttpGet("global/por-ano")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar(int anoLetivo, long dreId, long ueId, Modalidade modalidade, int semestre, [FromServices] IObterDashboardFrequenciaPorAnoUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade, semestre));
        }

        [HttpGet("global/dre")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoFrequenciaGlobalPorDREDto>), 200)]
        [Permissao(Permissao.PDA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaGlobalPorDre([FromQuery] FiltroGraficoFrequenciaGlobalPorDREDto filtro, [FromServices] IObterDadosDashboardFrequenciaPorDreUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("ausencias/motivo")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoBaseDto>), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> AusenciasPorMotivo(int anoLetivo, long dreId, long ueId, Modalidade modalidade, string ano, long turmaId, int semestre, [FromServices] IObterDashboardFrequenciaAusenciasPorMotivoUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade, ano, turmaId, semestre));
        }

        [HttpGet("ausencias/justificativas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoAusenciasComJustificativaResultadoDto>), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAusenciasComJustificativa(int anoLetivo, long dreId, long ueId, Modalidade modalidade, int semestre, [FromServices] IObterDadosDashboardAusenciasComJustificativaUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade, semestre));
        }

        [HttpGet("anos/{AnoLetivo}/dres/{dreId}/ues/{ueId}/modalidades/{modalidade}/anoTurma/{anoTurma}/consolidado-diario")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(GraficoFrequenciaAlunoDto), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciasConsolidadacaoDiariaPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade, string anoTurma, [FromQuery] int semestre, DateTime dataAula, bool visaoDre, [FromServices] IObterDadosDashboardFrequenciaDiariaPorAnoTurmaUseCase useCase)
        {
            var dto = new FrequenciasConsolidadacaoPorTurmaEAnoDto()
            {
                AnoLetivo = anoLetivo,
                AnoTurma = anoTurma,
                Semestre = semestre,
                DataAula = dataAula,
                DreId = dreId,
                Modalidade = modalidade,
                UeId = ueId,
                VisaoDre = visaoDre
            };

            return Ok(await useCase.Executar(dto));
        }       
        
        [HttpGet("anos/{AnoLetivo}/dres/{dreId}/ues/{ueId}/modalidades/{modalidade}/anoTurma/{anoTurma}/consolidado-semanal-mensal")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(GraficoFrequenciaAlunoDto), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciasConsolidacaoSemanalMensalPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade, TipoConsolidadoFrequencia tipoConsolidadoFrequencia, string anoTurma, [FromQuery] int semestre,DateTime? dataInicio, DateTime? datafim, int? mes, bool visaoDre, [FromServices] IObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase useCase)
        {
            var dto = new FrequenciasConsolidadacaoPorTurmaEAnoDto()
            {
                AnoLetivo = anoLetivo,
                AnoTurma = anoTurma,
                Semestre = semestre,
                DreId = dreId,
                Modalidade = modalidade,
                UeId = ueId,
                VisaoDre = visaoDre
            };

            return Ok(await useCase.Executar(dto, dataInicio, datafim,mes, tipoConsolidadoFrequencia));
        }    

        [HttpGet("filtro/anos/{AnoLetivo}/semanas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<GraficoAusenciasComJustificativaResultadoDto>), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSemanasFiltro(int anoLetivo, [FromServices] IObterFiltroSemanaUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo));
        }

        [HttpPost("consolidar")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(bool), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> ConsolidarFrequenciasParaDashBorad([FromQuery] FiltroConsolicacaoDiariaDashBoardFrequenciaDTO filtro, [FromServices] IExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase useCase)
        {
            await useCase.Executar(filtro);
            return Ok();
        }
    }
}
