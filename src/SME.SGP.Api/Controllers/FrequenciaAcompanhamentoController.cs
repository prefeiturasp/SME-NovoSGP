
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

        [HttpGet("turmas/{turmaId}/componentes-curriculares/{componenteCurricularId}/alunos/{alunoCodigo}/bimestres/{bimestre}/justificativas/semestre{semestre}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<JustificativaAlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 602)]
        [Permissao(Permissao.AFQ_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterJustificativasAlunoPorComponenteCurricular(long turmaId, long alunoCodigo, long componenteCurricularId, int bimestre, int? semestre, [FromServices] IObterJustificativasAlunoPorComponenteCurricularUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroJustificativasAlunoPorComponenteCurricular(turmaId, componenteCurricularId, alunoCodigo, bimestre, semestre)));
        }

        [HttpGet("turmas/{turmaId}/semestres/{semestre}/alunos/{alunoCodigo}")]
        [ProducesResponseType(typeof(FrequenciaAlunoBimestreDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 602)]
        [Permissao(Permissao.AFQ_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterInformacoesDeFrequenciaAlunoPorSemestre(long turmaId, int semestre, long alunoCodigo,[FromQuery] long componenteCurricularId,  [FromServices] IObterInformacoesDeFrequenciaAlunoPorSemestreUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroTurmaAlunoSemestreDto(turmaId, alunoCodigo, semestre, componenteCurricularId)));
        }
    }
}