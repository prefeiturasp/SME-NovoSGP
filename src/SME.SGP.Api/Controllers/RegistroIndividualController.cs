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
    [Route("api/v1/registros-individuais")]
    [Authorize("Bearer")]
    public class RegistroIndividualController : ControllerBase
    {

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.REI_C, Policy = "Bearer")]
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
        [Permissao(Permissao.REI_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromServices] IInserirRegistroIndividualUseCase useCase, [FromBody] InserirRegistroIndividualDto registroIndividualDto)
        {
            return Ok(await useCase.Executar(registroIndividualDto));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.REI_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromServices] IAlterarRegistroIndividualUseCase useCase, [FromBody] AlterarRegistroIndividualDto registroIndividualDto, long id)
        {
            registroIndividualDto.Id = id;
            return Ok(await useCase.Executar(registroIndividualDto));
        }

        [HttpGet("turmas/{turmaId}/componentes-curriculares/{componenteCurricularId}/alunos")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDadosBasicosDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.REI_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarAlunosPorTurma([FromServices] IListarAlunosDaTurmaRegistroIndividualUseCase useCase, long turmaId, long componenteCurricularId)
        {
            return Ok(await useCase.Executar(new FiltroRegistroIndividualBase(turmaId, componenteCurricularId)));
        }

        [HttpGet("turmas/{turmaId}/alunos/{alunoCodigo}/componentes-curriculares/{componenteCurricularId}/data/{data}")]
        [ProducesResponseType(typeof(RegistrosIndividuaisPeriodoDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.REI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorAlunoData([FromServices] IObterRegistroIndividualPorAlunoDataUseCase useCase, long turmaId, long alunoCodigo, long componenteCurricularId, DateTime data)
        {
            return Ok(await useCase.Executar(new FiltroRegistroIndividualAlunoData(turmaId, componenteCurricularId, alunoCodigo, data)));
        }

        [HttpGet("turmas/{turmaId}/alunos/{alunoCodigo}/componentes-curriculares/{componenteCurricularId}/dataInicio/{dataInicio}/dataFim/{dataFim}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<RegistroIndividualDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.REI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorAlunoPeriodo([FromServices] IObterRegistrosIndividuaisPorAlunoPeriodoUseCase useCase, long turmaId, long alunoCodigo, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            return Ok(await useCase.Executar(new FiltroRegistroIndividualAlunoPeriodo(turmaId, componenteCurricularId, alunoCodigo, dataInicio, dataFim)));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.REI_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir(long id, [FromServices] IExcluirRegistroIndividualUseCase excluirRegistroIndividualUseCase)
        {
            return Ok(await excluirRegistroIndividualUseCase.Executar(id));
        }

        [HttpGet("sugestoes-topicos/{mes}")]
        [ProducesResponseType(typeof(SugestaoTopicoRegistroIndividualDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.REI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSugestaoTopico([FromServices] IObterSugestaoTopicoRegistroIndividualPorMesUseCase useCase, int mes)
        {
            return Ok(await useCase.Executar(mes));
        }
    }
}

