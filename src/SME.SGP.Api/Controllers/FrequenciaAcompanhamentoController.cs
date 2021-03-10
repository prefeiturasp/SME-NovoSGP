﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/frequencias/acompanhamentos")]
    [Authorize("Bearer")]
    public class FrequenciaAcompanhamentoController : ControllerBase
    {
        [HttpGet("")]
        [ProducesResponseType(typeof(FrequenciaAlunosPorBimestreDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 602)]
        [Permissao(Permissao.AFQ_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterInformacoesDeFrequenciaAlunosPorBimestre([FromQuery] ObterFrequenciaAlunosPorBimestreDto dto, [FromServices] IObterInformacoesDeFrequenciaAlunosPorBimestreUseCase useCase)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpGet("turmas/{turmaId}/componentes-curriculares/{componenteCurricularId}/alunos/{alunoCodigo}/justificativas")]
        [ProducesResponseType(typeof(IEnumerable<JustificativaAlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 602)]
        [Permissao(Permissao.AFQ_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterJustificativasAlunoPorComponenteCurricular(long turmaId, long alunoCodigo, long componenteCurricularId, [FromServices] IObterJustificativasAlunoPorComponenteCurricularUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroJustificativasAlunoPorComponenteCurricular(turmaId, componenteCurricularId, alunoCodigo)));
        }
    }
}