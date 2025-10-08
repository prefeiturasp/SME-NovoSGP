using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Turma;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/periodo-escolar")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class PeriodoEscolarController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(PeriodoEscolarDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PE_C, Policy = "Bearer")]
        public async Task<IActionResult> Get(long codigoTipoCalendario, [FromServices] IConsultasPeriodoEscolar consultas)
        {
            var periodoEscolar = await consultas.ObterPorTipoCalendario(codigoTipoCalendario);

            if (periodoEscolar.EhNulo())
                return NoContent();

            return Ok(periodoEscolar);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PE_I, Permissao.PE_A, Policy = "Bearer")]
        public async Task<IActionResult> Post([FromBody] PeriodoEscolarListaDto periodos, [FromServices] IComandosPeriodoEscolar comandoPeriodo)
        {
            await comandoPeriodo.Salvar(periodos);
            return Ok();
        }

        [HttpGet("bimestres/{bimestre}/turmas/{turmaCodigo}/aberto")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]        
        public async Task<IActionResult> PeriodoEmAberto(string turmaCodigo, int bimestre, [FromQuery] DateTime dataReferencia, [FromServices] IConsultasTurma consultas)
        {
            var dataConsulta = dataReferencia == DateTime.MinValue ? DateTime.Today : dataReferencia;
            return Ok(await consultas.TurmaEmPeriodoAberto(turmaCodigo, dataConsulta, bimestre));
        }

        [HttpGet("turmas/{turmaCodigo}/bimestres/aberto")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<PeriodoEscolarAbertoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]        
        public async Task<IActionResult> PeriodosEmAberto(string turmaCodigo, [FromQuery] DateTime dataReferencia, [FromServices] IConsultasTurma consultas)
        {
            var dataConsulta = dataReferencia == DateTime.MinValue ? DateTime.Today : dataReferencia;
            return Ok(await consultas.PeriodosEmAbertoTurma(turmaCodigo, dataConsulta));
        }

        [HttpGet("modalidades/{modalidade}/bimestres/atual")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAtual(int modalidade, [FromServices] IConsultasPeriodoEscolar consultas)
        {
            return Ok(await consultas.ObterBimestre(DateTime.Today, (Dominio.Modalidade)modalidade));
        }

        [HttpGet("turmas/{turmaId}")]
        [ProducesResponseType(typeof(IEnumerable<PeriodoEscolarPorTurmaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterBimetresPeriodosEscolaresTurma([FromServices] IObterPeriodoEscolarPorTurmaUseCase useCase, long turmaId)
        {
            return Ok(await useCase.Executar(turmaId));
        }

        [HttpGet("modalidades/{modalidade}/ano-letivo/{AnoLetivo}/bimestres")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<PeriodoEscolarDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterBimestresModalidadeEAno(int modalidade, int anoLetivo, [FromServices] IObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase useCase, [FromQuery] int semestre = 0)
        {
            return Ok(await useCase.Executar((Dominio.Modalidade)modalidade, anoLetivo, semestre));
        }

        [HttpGet("turmas/{turmaId}/bimestres/atual")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(BimestreDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> BimestreAtual(long turmaId, [FromServices] IObterBimestreAtualPorTurmaIdUseCase useCase)
        {
            var bimestre = await useCase.Executar(turmaId);

            if (bimestre.NaoEhNulo())
                return Ok(bimestre);
            else
                return NoContent();
        }

        [HttpGet("turmas/{turmaCodigo}/periodo-letivo")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(PeriodoEscolarLetivoTurmaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterPeriodoLetivoTurma(string turmaCodigo, [FromServices] IObterPeriodoLetivoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(turmaCodigo));
        }

        [HttpGet("turmas/{turmaCodigo}/componentes-curriculares/{componenteCurricularId}/regencia/{ehRegencia}/bimestres/{bimestre}/")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<PeriodoEscolarComponenteDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterPeriodoPorComponente(string turmaCodigo, long componenteCurricularId, bool ehRegencia, int bimestre,[FromServices] IObterPeriodosPorComponenteUseCase useCase, [FromQuery] bool exibirDataFutura = false)
        {
            return Ok(await useCase.Executar(turmaCodigo, componenteCurricularId, ehRegencia, bimestre, exibirDataFutura));
        }
    }
}