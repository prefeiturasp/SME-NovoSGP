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
    [Route("api/v1/ocorrencias")]
    [Authorize("Bearer")]
    public class OcorrenciaController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<OcorrenciaListagemDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.OCO_C, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromServices] IListarOcorrenciasUseCase useCase, [FromQuery] FiltroOcorrenciaListagemDto dto)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OcorrenciaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.OCO_C, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromServices] IObterOcorrenciaUseCase useCase, long id)
        {
            var result = await useCase.Executar(id);
            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet]
        [Route("turma/{turmaId}/aluno/{codigoAluno}/semestre/{semestre}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<OcorrenciasPorAlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.OCO_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterOcorrenciasPorAluno(long codigoAluno, long turmaId, int semestre, [FromServices] IObterOcorrenciasPorAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroTurmaAlunoSemestreDto(turmaId, codigoAluno, semestre)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.OCO_I, Policy = "Bearer")]
        public async Task<IActionResult> Inserir([FromServices] IInserirOcorrenciaUseCase useCase, [FromBody] InserirOcorrenciaDto dto)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.OCO_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromServices] IAlterarOcorrenciaUseCase useCase, [FromBody] AlterarOcorrenciaDto dto)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.OCO_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir([FromBody] IEnumerable<long> ids, [FromServices] IExcluirOcorrenciaUseCase excluirOcorrenciaUseCase)
        {
            var retorno = await excluirOcorrenciaUseCase.Executar(ids);
            return Ok(retorno);
        }
    }
}