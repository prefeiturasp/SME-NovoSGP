using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api
{
    [ApiController]
    [Route("api/v1/registro-individual")]
    [Authorize("Bearer")]
    public class RegistroIndividualController : ControllerBase        
    {

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter([FromServices] IObterRegistroIndividualUseCase useCase, long id)
        {
            var result = await useCase.Executar(id);
            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromServices] IInserirRegistroIndividualUseCase useCase, [FromBody] InserirRegistroIndividualDto registroIndividualDto)
        {
            return Ok(await useCase.Executar(registroIndividualDto));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromServices] IAlterarRegistroIndividualUseCase useCase, [FromBody] AlterarRegistroIndividualDto registroIndividualDto)
        {
            return Ok(await useCase.Executar(registroIndividualDto));
        }

        [HttpGet("turmas/{turmaId}/componentes-curriculares/{componenteCurricularId}/alunos")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDadosBasicosDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarAlunosPorTurma([FromServices] IListarAlunosDaTurmaUseCase useCase, long turmaId, long componenteCurricularId)
        {
            return Ok(await useCase.Executar(new FiltroRegistroIndividualBase(turmaId, componenteCurricularId)));
        }

        [HttpGet("turmas/{turmaId}/alunos/{alunoCodigo}/componentes-curriculares/{componenteCurricular}/data/{data}")]
        [ProducesResponseType(typeof(RegistroIndividualDataDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorAlunoData([FromServices] IObterRegistroIndividuailPorAlunoDataUseCase useCase, long turmaId, long alunoCodigo, long componenteCurricularId, DateTime data)
        {
            return Ok(await useCase.Executar(new FiltroRegistroIndividualAlunoData(turmaId, componenteCurricularId, alunoCodigo, data)));
        }

        [HttpGet("turmas/{turmaCodigo}/alunos/{alunoCodigo}/componentes-curriculares/{componenteCurricular}/dataInicio/{dataInicio}/dataFim/{dataFim}")]
        [ProducesResponseType(typeof(IEnumerable<RegistroIndividualDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorAlunoPeriodo([FromServices] IObterRegistrosIndividuaisPorAlunoPeriodoUseCase useCase, long turmaId, long alunoCodigo, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            return Ok(await useCase.Executar(new FiltroRegistroIndividualAlunoPeriodo(turmaId, componenteCurricularId, alunoCodigo, dataInicio, dataFim)));
        }
    }
}

