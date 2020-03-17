﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.Background.Core;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v1/calendarios")]
    public class FrequenciaController : ControllerBase
    {
        [HttpGet("frequencias")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [Permissao(Permissao.PDA_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar(long aulaId, [FromServices] IConsultasFrequencia consultasFrequencia)
        {
            var retorno = await consultasFrequencia.ObterListaFrequenciaPorAula(aulaId);

            if (retorno == null)
                return NoContent();

            return Ok(retorno);
        }

        [HttpPost("frequencias/notificar")]
        public IActionResult Notificar()
        {
            Cliente.Executar<IServicoNotificacaoFrequencia>(c => c.ExecutaNotificacaoFrequencia());
            return Ok();
        }

        [HttpGet("frequencias/aulas/datas/{anoLetivo}/turmas/{turmaId}/disciplinas/{disciplinaId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [Permissao(Permissao.PDA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(int anoLetivo, string turmaId, string disciplinaId, [FromServices] IConsultasAula consultasAula)
        {
            return Ok(await consultasAula.ObterDatasDeAulasPorCalendarioTurmaEDisciplina(anoLetivo, turmaId, disciplinaId));
        }

        [HttpPost("frequencias")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(200)]
        [Permissao(Permissao.PDA_I, Policy = "Bearer")]
        public async Task<IActionResult> Registrar([FromBody] FrequenciaDto frequenciaDto, [FromServices] IComandoFrequencia comandoFrequencia)
        {
            await comandoFrequencia.Registrar(frequenciaDto);
            return Ok();
        }

        [HttpGet("frequencias/ausencias/turmas/{turmaId}/disciplinas/{disciplinaId}/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<AlunoAusenteDto>), 200)]
        public async Task<IActionResult> ObterAusenciasTurma(string turmaId, string disciplinaId, int bimestre, [FromServices] IConsultasFrequencia consultasFrequencia)
            => Ok(await consultasFrequencia.ObterListaAlunosComAusencia(turmaId, disciplinaId, bimestre));
    }
}