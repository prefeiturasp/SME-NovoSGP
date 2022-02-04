﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v1/calendarios")]
    public class FrequenciaController : ControllerBase
    {
        private readonly IMediator mediator;

        public FrequenciaController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("frequencias")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [Permissao(Permissao.PDA_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar([FromQuery] FiltroFrequenciaDto filtro, [FromServices] IObterFrequenciaPorAulaUseCase useCase)
        {
            var retorno = await useCase.Executar(filtro);            
            
            if (retorno == null)
                return NoContent();

            return Ok(retorno);
        }

        [HttpPost("frequencias/notificar")]
        public async Task<IActionResult> Notificar()
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.NotifificarRegistroFrequencia, null, Guid.NewGuid(), null));
            
            return Ok();
        }

        [HttpPost("frequencias/notificar/alunos/faltosos")]
        public async Task<IActionResult> NotificarAlunosFaltosos([FromServices] IServicoNotificacaoFrequencia servico)
        {
            await servico.NotificarAlunosFaltosos();
            
            return Ok();
        }

        [HttpPost("frequencias/notificar/alunos/faltosos/bimestre")]
        public async Task<IActionResult> NotificarAlunosFaltososBimestre([FromServices] IServicoNotificacaoFrequencia servico)
        {
            await servico.NotificarAlunosFaltososBimestre();
            
            return Ok();
        }

        [HttpGet("frequencias/aulas/datas/{anoLetivo}/turmas/{turmaId}/disciplinas/{disciplinaId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [Permissao(Permissao.PDA_C, Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(int anoLetivo, string turmaId, string disciplinaId, [FromServices] IConsultasAula consultasAula)
        {
            return Ok(await consultasAula.ObterDatasDeAulasPorCalendarioTurmaEDisciplina(anoLetivo, turmaId, disciplinaId));
        }

        [HttpGet("frequencias/aulas/datas/turmas/{turmaCodigo}/componente/{componenteCurricularCodigo}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [Permissao(Permissao.PDA_C, Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDatasDeAulasPorCalendarioTurmaEComponenteCurricular(string turmaCodigo, string componenteCurricularCodigo, [FromServices] IObterDatasAulasPorTurmaEComponenteUseCase useCase)
        {
            return Ok(await useCase.Executar(new ConsultaDatasAulasDto(turmaCodigo, componenteCurricularCodigo)));
        }

        [HttpPost("frequencias")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [Permissao(Permissao.PDA_I, Policy = "Bearer")]
        public async Task<IActionResult> Registrar([FromBody] FrequenciaDto frequenciaDto, [FromServices] IInserirFrequenciaUseCase useCase)
        {
            return Ok(await useCase.Executar(frequenciaDto));
        }

        [HttpGet("frequencias/ausencias/turmas/{turmaId}/disciplinas/{disciplinaId}/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<AlunoAusenteDto>), 200)]
        public async Task<IActionResult> ObterAusenciasTurma(string turmaId, string disciplinaId, int bimestre, [FromServices] IConsultasFrequencia consultasFrequencia)
            => Ok(await consultasFrequencia.ObterListaAlunosComAusencia(turmaId, disciplinaId, bimestre));

        [HttpGet("frequencias/alunos/{alunoCodigo}/turmas/{turmaCodigo}/geral")]
        [ProducesResponseType(typeof(double), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo, [FromServices] IConsultasFrequencia consultasFrequencia)
             => Ok(await consultasFrequencia.ObterFrequenciaGeralAluno(alunoCodigo, turmaCodigo));

        [HttpGet("frequencias/alunos/{alunoCodigo}/turmas/{turmaCodigo}/semestre/{semestre}/geral")]
        [ProducesResponseType(typeof(double), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo, int semestre, [FromServices] IConsultasFrequencia consultasFrequencia)
             => Ok(await consultasFrequencia.ObterFrequenciaGeralAluno(alunoCodigo, turmaCodigo, semestre: semestre));

        [AllowAnonymous]
        [HttpGet("frequencias/ausencias-motivos")]
        [ProducesResponseType(typeof(IEnumerable<AusenciaMotivoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterAusenciaMotivoPorAlunoTurmaBimestreAno([FromQuery] string codigoAluno, [FromQuery] string codigoTurma, [FromQuery] short bimestre, [FromQuery] short anoLetivo, [FromServices] IConsultasFrequencia consultasFrequencia)
             => Ok(await consultasFrequencia.ObterAusenciaMotivoPorAlunoTurmaBimestreAno(codigoAluno, codigoTurma, bimestre, anoLetivo));

        [HttpPost("frequencias/calcular")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CalcularFrequencia([FromBody] CalcularFrequenciaDto calcularFrequenciaDto, [FromServices] ICalculoFrequenciaTurmaDisciplinaUseCase calculoFrequenciaTurmaDisciplinaUseCase)
        {
            await calculoFrequenciaTurmaDisciplinaUseCase.IncluirCalculoFila(calcularFrequenciaDto);
            
            return Ok();
        }

        [HttpGet("frequencias/pre-definidas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ObterFrequenciasPreDefinidas([FromQuery] FiltroFrequenciaPreDefinidaDto filtro, [FromServices] IObterFrequenciasPreDefinidasUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));           
        }

        [HttpGet("frequencias/tipos")]
        [ProducesResponseType(typeof(TipoFrequenciaDto), 500)]
        [ProducesResponseType(typeof(TipoFrequenciaDto), 601)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ObterTiposFrequenciasPorModalidade([FromQuery] TipoFrequenciaFiltroDto filtro, [FromServices] IObterTiposFrequenciasUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpPost("frequencias/conciliar")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ConciliarFrequencia([FromQuery] DateTime dataReferencia, string turmaCodigo)
        {
            var mensagem = new ConciliacaoFrequenciaTurmasSyncDto(dataReferencia, turmaCodigo);
            
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaConciliacaoFrequenciaTurmasSync, mensagem, Guid.NewGuid(), null));
            
            return Ok();
        }

        [HttpPost("frequencias/turmas/conciliar")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ConciliarFrequenciaTurmas([FromQuery] DateTime dataReferencia, string[] turmasCodigos)
        {
            foreach(var turmaCodigo in turmasCodigos)
            {
                var mensagem = new ConciliacaoFrequenciaTurmasSyncDto(dataReferencia, turmaCodigo);

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaConciliacaoFrequenciaTurmasSync, mensagem, Guid.NewGuid(), null));
            }

            return Ok();
        }

        [HttpPost("frequencias/consolidar")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ConsolidarFrequencia([FromQuery] int ano, [FromServices] IExecutaConsolidacaoFrequenciaPorAnoUseCase useCase)
        {
            await useCase.Executar(ano);
           
            return Ok();
        }

        [HttpGet("frequencias/turmas/{turmaCodigo}/alunos/{alunoCodigo}/componentes-curriculares/{componenteCurricularId}")]
        [ProducesResponseType(typeof(IEnumerable<FrequenciaAluno>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricular(string turmaCodigo, string alunoCodigo, string componenteCurricularId, [FromQuery] int[] bimestres, [FromServices] IObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase useCase)
        {
            return Ok(await useCase.Executar(new FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto(turmaCodigo, alunoCodigo, bimestres, componenteCurricularId)));
        }

        [HttpPost("frequencias/salvar")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> SalvarFrequencia([FromBody] IEnumerable<FrequenciaSalvarAulaAlunosDto> frequenciaListaoDto, [FromServices] IInserirFrequenciaListaoUseCase useCase)
        {
            return Ok(await useCase.Executar(frequenciaListaoDto));
        }

        [HttpGet("frequencias/por-periodo")]
        [ProducesResponseType(typeof(RegistroFrequenciaPorDataPeriodoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterFrequenciasPorPeriodo([FromQuery] FiltroFrequenciaPorPeriodoDto filtro, [FromServices] IObterFrequenciasPorPeriodoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }
}