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
        public async Task<IActionResult> ObterFrequenciasConsolidadasPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade, int tipoPeriodoDashboard,[FromQuery] int semestre, int anoTurma, DateTime dataInicio, DateTime datafim, int mes, [FromServices] IObterDadosDashboardFrequenciaPorAnoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade, semestre, anoTurma, dataInicio, datafim, mes, tipoPeriodoDashboard));
        }

        [HttpGet("anos/{anoLetivo}/modalidades/{modalidade}/consolidado/dres")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(GraficoFrequenciaAlunoDto), 200)]
        public async Task<IActionResult> ObterFrequenciasConsolidadasPorDre(int anoLetivo, int modalidade, [FromQuery] int semestre, [FromQuery] int anoTurma, [FromQuery] DateTime dataInicio, [FromQuery] DateTime datafim, [FromQuery] int tipoPeriodoDashboard)
        {
            var dadosGraficoDiario = new GraficoFrequenciaAlunoDto
            {
                QuantidadeFrequenciaRegistrada = 2000,
                PorcentagemAulas = 20,
                DadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>()
                {
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "BT", Quantidade = 120},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "BT", Quantidade = 170},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "BT", Quantidade = 130},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "BT", Quantidade = 420},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "CL", Quantidade = 200},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "CL", Quantidade = 150},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "CL", Quantidade = 160},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "CL", Quantidade = 480},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "CS", Quantidade = 300},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "CS", Quantidade = 267},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "CS", Quantidade = 180},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "CS", Quantidade = 567},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "FB", Quantidade = 400},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "FB", Quantidade = 345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "FB", Quantidade = 200},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "FB", Quantidade = 867},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "G", Quantidade = 500},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "EG", Quantidade = 456},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "G", Quantidade = 300},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "G", Quantidade = 1356},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "IP", Quantidade = 600},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "IP", Quantidade = 500},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "IP", Quantidade = 400},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "IP", Quantidade = 1900},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "IQ", Quantidade = 200},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "IQ", Quantidade = 534},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "IQ", Quantidade = 130},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "IQ", Quantidade = 1345},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "JT", Quantidade = 120},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "JT", Quantidade = 267},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "JT", Quantidade = 148},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "JT", Quantidade = 765},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "PE", Quantidade = 150},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "PE", Quantidade = 345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "PE", Quantidade = 187},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "PE", Quantidade = 876},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "PJ", Quantidade = 150},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "PJ", Quantidade = 345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "PJ", Quantidade = 187},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "PJ", Quantidade = 876},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "SA", Quantidade = 150},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "SA", Quantidade = 345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "SA", Quantidade = 187},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "SA", Quantidade = 876},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "SM", Quantidade = 150},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "SM", Quantidade = 345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "SM", Quantidade = 187},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "SM", Quantidade = 876},

                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Presentes", TurmaAno = "MP", Quantidade = 150},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Remotos", TurmaAno = "MP", Quantidade = 345},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Ausentes", TurmaAno = "MP", Quantidade = 187},
                    new DadosRetornoFrequenciaAlunoDashboardDto(){Descricao = "Total de Estudantes", TurmaAno = "MP", Quantidade = 876},                    
                }
            };           

            return Ok(dadosGraficoDiario);

        }
    }
}
