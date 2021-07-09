using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
   // [Authorize("Bearer")]
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
        public async Task<IActionResult> ObterAusenciasComJustificativa(int anoLetivo, long dreId, long ueId, Modalidade modalidade, int semestre, [FromServices] IObterDadosDashboardAusenciasComJustificativaUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade, semestre));
        }

        [HttpGet("anos/{anoLetivo}/dres/{dreId}/ues/{ueId}/modalidades/{modalidade}/consolidado/anos-turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(GraficoFrequenciaAlunoDto), 200)]
        public async Task<IActionResult> ObterFrequenciasPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade,[FromQuery] int semestre, [FromQuery] int anoTurma, [FromQuery] DateTime dataInicio, [FromQuery] DateTime datafim, [FromQuery] int tipoPeriodoDashboard)
        {
            var dadosGraficoDiario = new GraficoFrequenciaAlunoDto
            {
                QuantidadeFrequenciaRegistrada = 2000,
                PorcentagemAulas = 25,
                DadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>()
                {
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-1", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-2", QuantidadePresentes = 200, QuantidadeRemotos = 150, QuantidadeAusentes = 160, TotalAlunos = 480},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-3", QuantidadePresentes = 300, QuantidadeRemotos = 267, QuantidadeAusentes = 180, TotalAlunos = 567},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-4", QuantidadePresentes = 400, QuantidadeRemotos = 345, QuantidadeAusentes = 200, TotalAlunos = 876},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-5", QuantidadePresentes = 500, QuantidadeRemotos = 456, QuantidadeAusentes = 300, TotalAlunos = 1356},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-6", QuantidadePresentes = 600, QuantidadeRemotos = 500, QuantidadeAusentes = 400, TotalAlunos = 1900},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-7", QuantidadePresentes = 200, QuantidadeRemotos = 534, QuantidadeAusentes = 130, TotalAlunos = 1345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-8", QuantidadePresentes = 120, QuantidadeRemotos = 267, QuantidadeAusentes = 148, TotalAlunos = 765},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-9", QuantidadePresentes = 150, QuantidadeRemotos = 345, QuantidadeAusentes = 187, TotalAlunos = 876}
                }
            };

            var dadosGraficoSemanal = new GraficoFrequenciaAlunoDto
            {
                QuantidadeFrequenciaRegistrada = 20000,
                PorcentagemAulas = 25,
                DadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>()
                {
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-1", QuantidadePresentes = 200, QuantidadeRemotos = 170, QuantidadeAusentes = 300, TotalAlunos = 600},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-2", QuantidadePresentes = 400, QuantidadeRemotos = 150, QuantidadeAusentes = 345, TotalAlunos = 765},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-3", QuantidadePresentes = 400, QuantidadeRemotos = 267, QuantidadeAusentes = 400, TotalAlunos = 876},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-4", QuantidadePresentes = 300, QuantidadeRemotos = 345, QuantidadeAusentes = 123, TotalAlunos = 987},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-5", QuantidadePresentes = 500, QuantidadeRemotos = 456, QuantidadeAusentes = 300, TotalAlunos = 890},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-6", QuantidadePresentes = 600, QuantidadeRemotos = 500, QuantidadeAusentes = 400, TotalAlunos = 987},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-7", QuantidadePresentes = 200, QuantidadeRemotos = 534, QuantidadeAusentes = 130, TotalAlunos = 945},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-8", QuantidadePresentes = 800, QuantidadeRemotos = 267, QuantidadeAusentes = 150, TotalAlunos = 1345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-9", QuantidadePresentes = 750, QuantidadeRemotos = 345, QuantidadeAusentes = 187, TotalAlunos = 1678}
                }
            };

            var dadosGraficoMensal = new GraficoFrequenciaAlunoDto
            {
                QuantidadeFrequenciaRegistrada = 65000,
                PorcentagemAulas = 25,
                DadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>()
                {
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-1", QuantidadePresentes = 800, QuantidadeRemotos = 546, QuantidadeAusentes = 130, TotalAlunos = 1234},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-2", QuantidadePresentes = 765, QuantidadeRemotos = 654, QuantidadeAusentes = 160, TotalAlunos = 1467},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-3", QuantidadePresentes = 345, QuantidadeRemotos = 765, QuantidadeAusentes = 180, TotalAlunos = 1678},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-4", QuantidadePresentes = 768, QuantidadeRemotos = 345, QuantidadeAusentes = 200, TotalAlunos = 1567},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-5", QuantidadePresentes = 987, QuantidadeRemotos = 834, QuantidadeAusentes = 300, TotalAlunos = 1356},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-6", QuantidadePresentes = 678, QuantidadeRemotos = 934, QuantidadeAusentes = 400, TotalAlunos = 1900},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-7", QuantidadePresentes = 543, QuantidadeRemotos = 658, QuantidadeAusentes = 130, TotalAlunos = 1345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-8", QuantidadePresentes = 987, QuantidadeRemotos = 938, QuantidadeAusentes = 148, TotalAlunos = 1678},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "EF-9", QuantidadePresentes = 615, QuantidadeRemotos = 734, QuantidadeAusentes = 187, TotalAlunos = 1654}
                }
            };

            if(tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Diario)
                return Ok(dadosGraficoDiario);

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Semanal)
                return Ok(dadosGraficoSemanal);

            return Ok(dadosGraficoMensal);
        }

        [HttpGet("anos/{anoLetivo}/modalidades/{modalidade}/consolidado/dres")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(GraficoFrequenciaAlunoDto), 200)]
        public async Task<IActionResult> ObterFrequenciasAlunosPorDre(int anoLetivo, int modalidade, [FromQuery] int semestre, [FromQuery] int anoTurma, [FromQuery] DateTime dataInicio, [FromQuery] DateTime datafim, [FromQuery] int tipoPeriodoDashboard)
        {
            var dadosGraficoDiario = new GraficoFrequenciaAlunoDto
            {
                QuantidadeFrequenciaRegistrada = 2000,
                PorcentagemAulas = 20,
                DadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>()
                {
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "BT", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "CL", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "CS", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "FB", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "G", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "IP", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "IQ", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "JT", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "PE", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "PJ", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "SA", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "SM", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "MP", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420}
                }
            };

            var dadosGraficosSemanal = new GraficoFrequenciaAlunoDto
            {
                QuantidadeFrequenciaRegistrada = 35000,
                PorcentagemAulas = 25,
                DadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>()
                {
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "BT", QuantidadePresentes = 500, QuantidadeRemotos = 600, QuantidadeAusentes = 400, TotalAlunos = 1500},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "CL", QuantidadePresentes = 800, QuantidadeRemotos = 500, QuantidadeAusentes = 600, TotalAlunos = 1900},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "CS", QuantidadePresentes = 300, QuantidadeRemotos = 500, QuantidadeAusentes = 200, TotalAlunos = 1000},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "FB", QuantidadePresentes = 600, QuantidadeRemotos = 200, QuantidadeAusentes = 500, TotalAlunos = 1300},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "G", QuantidadePresentes = 200, QuantidadeRemotos = 300, QuantidadeAusentes = 250, TotalAlunos = 750},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "IP", QuantidadePresentes = 350, QuantidadeRemotos = 650, QuantidadeAusentes = 450, TotalAlunos = 1450},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "IQ", QuantidadePresentes = 600, QuantidadeRemotos = 200, QuantidadeAusentes = 500, TotalAlunos = 1300},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "JT", QuantidadePresentes = 800, QuantidadeRemotos = 500, QuantidadeAusentes = 600, TotalAlunos = 1900},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "PE", QuantidadePresentes = 200, QuantidadeRemotos = 300, QuantidadeAusentes = 250, TotalAlunos = 750},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "PJ", QuantidadePresentes = 120, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 420},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "SA", QuantidadePresentes = 800, QuantidadeRemotos = 500, QuantidadeAusentes = 600, TotalAlunos = 1900},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "SM", QuantidadePresentes = 200, QuantidadeRemotos = 300, QuantidadeAusentes = 250, TotalAlunos = 750},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "MP", QuantidadePresentes = 600, QuantidadeRemotos = 200, QuantidadeAusentes = 500, TotalAlunos = 1300}
                }
            };

            var dadosGraficosMensal = new GraficoFrequenciaAlunoDto
            {
                QuantidadeFrequenciaRegistrada = 65000,
                PorcentagemAulas = 25,
                DadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>()
                {
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "BT", QuantidadePresentes = 900, QuantidadeRemotos = 600, QuantidadeAusentes = 400, TotalAlunos = 2700},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "CL", QuantidadePresentes = 1200, QuantidadeRemotos = 500, QuantidadeAusentes = 600, TotalAlunos = 2365},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "CS", QuantidadePresentes = 964, QuantidadeRemotos = 500, QuantidadeAusentes = 200, TotalAlunos = 2654},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "FB", QuantidadePresentes = 1234, QuantidadeRemotos = 200, QuantidadeAusentes = 500, TotalAlunos = 1908},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "G", QuantidadePresentes = 1567, QuantidadeRemotos = 300, QuantidadeAusentes = 250, TotalAlunos = 2876},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "IP", QuantidadePresentes = 1876, QuantidadeRemotos = 650, QuantidadeAusentes = 450, TotalAlunos = 2765},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "IQ", QuantidadePresentes = 1456, QuantidadeRemotos = 200, QuantidadeAusentes = 500, TotalAlunos = 2654},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "JT", QuantidadePresentes = 1478, QuantidadeRemotos = 500, QuantidadeAusentes = 600, TotalAlunos = 2435},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "PE", QuantidadePresentes = 986, QuantidadeRemotos = 300, QuantidadeAusentes = 250, TotalAlunos = 2765},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "PJ", QuantidadePresentes = 894, QuantidadeRemotos = 170, QuantidadeAusentes = 130, TotalAlunos = 2345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "SA", QuantidadePresentes = 800, QuantidadeRemotos = 500, QuantidadeAusentes = 600, TotalAlunos = 1900},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "SM", QuantidadePresentes = 1754, QuantidadeRemotos = 300, QuantidadeAusentes = 250, TotalAlunos = 2675},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "MP", QuantidadePresentes = 1286, QuantidadeRemotos = 200, QuantidadeAusentes = 500, TotalAlunos = 2546}
                }
            };

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Diario)
                return Ok(dadosGraficoDiario);
            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Semanal)
                return Ok(dadosGraficosSemanal);

            return Ok(dadosGraficosMensal);

        }
    }
}
