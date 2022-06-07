
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("turmas/{turmaId}/componentes-curriculares/{componenteCurricularId}/alunos/{alunoCodigo}/bimestres/{bimestre}/justificativas/semestre/{semestre}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<JustificativaAlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 602)]
        [Permissao(Permissao.AFQ_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterJustificativasAlunoPorComponenteCurricular(long turmaId, long alunoCodigo, long componenteCurricularId, int bimestre, int? semestre, [FromServices] IObterJustificativasAlunoPorComponenteCurricularUseCase useCase)
        {
            var retorno = await useCase.Executar(new FiltroJustificativasAlunoPorComponenteCurricular(turmaId, componenteCurricularId, alunoCodigo, bimestre, semestre));
            return Ok(retorno);
        }

        [HttpGet("turma/{turmaId}/componente-curricular/{componenteCurricularId}/aluno/{alunoCodigo}/bimestre/{bimestre}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>),200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 602)]
        [Permissao(Permissao.AFQ_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFrequenciaDiariaAluno(long turmaId,long componenteCurricularId, long alunoCodigo, int bimestre)
        {
            IEnumerable<FrequenciaDiariaAlunoDto> retorno = new List<FrequenciaDiariaAlunoDto>
            {
                new FrequenciaDiariaAlunoDto(1360815,DateTime.Now.AddDays(1),10,2,5,3,"Teste1"),
                new FrequenciaDiariaAlunoDto(1360815,DateTime.Now.AddDays(2),10,2,5,3,"Atestado Médico do Aluno2"),
                new FrequenciaDiariaAlunoDto(null,DateTime.Now.AddDays(3),10,2,5,3,null),
            };
            var paginacao = new PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>()
            {
                TotalPaginas = 1,
                TotalRegistros = 3,
                Items = retorno.OrderByDescending(x => x.DataAula)
            };
            return Ok(paginacao);
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