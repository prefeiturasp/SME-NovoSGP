using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/anotacoes/alunos")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class AnotacaoFrequenciaAlunoController : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AnotacaoFrequenciaAlunoCompletoDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_C, Permissao.PDA_I, Permissao.PDA_A, Permissao.PDA_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterJustificativaCompleto(long id, [FromServices] IObterAnotacaoFrequenciaAlunoPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(id));
        }

        [HttpGet("{codigoAluno}/aulas/{aulaId}")]
        [ProducesResponseType(typeof(AnotacaoFrequenciaAlunoDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_C, Permissao.PDA_I, Permissao.PDA_A, Permissao.PDA_E, Policy = "Bearer")]
        public async Task<IActionResult> BuscarPorId(string codigoAluno, long aulaId, [FromServices] IObterAnotacaoFrequenciaAlunoUseCase useCase)
        {
            var anotacao = await useCase.Executar(new FiltroAnotacaoFrequenciaAlunoDto(codigoAluno, aulaId));

            if (anotacao.EhNulo())
                return NoContent();

            return Ok(anotacao);
        }

        [HttpGet("{codigoAluno}/data")]
        [ProducesResponseType(typeof(AnotacaoAlunoAulaPorPeriodoDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CCEA_NAAPA_C, Permissao.RABA_NAAPA_C, Permissao.PDA_C, Permissao.PDA_I, Permissao.PDA_A, Permissao.PDA_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorAlunoPorPeriodo(string codigoAluno, DateTime dataInicio, DateTime dataFim, [FromServices] IObterAnotacaoFrequenciaAlunoPorPeriodoUseCase useCase)
        {
            var anotacao = await useCase.Executar(new FiltroAnotacaoFrequenciaAlunoPorPeriodoDto(codigoAluno, dataInicio, dataFim));

            if (anotacao.EhNulo())
                return NoContent();

            return Ok(anotacao);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_I, Permissao.PDA_A, Permissao.PDA_E, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] SalvarAnotacaoFrequenciaAlunoDto dto, [FromServices] ISalvarAnotacaoFrequenciaAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_I, Permissao.PDA_A, Permissao.PDA_E, Policy = "Bearer")]
        public async Task<IActionResult> Alterar(long id, [FromBody] AlterarAnotacaoFrequenciaAlunoDto dto, [FromServices] IAlterarAnotacaoFrequenciaAlunoUseCase useCase)
        {
            dto.Id = id;
            return Ok(await useCase.Executar(dto));
        }

        [HttpGet("motivos-ausencia")]
        [ProducesResponseType(typeof(IEnumerable<OpcaoDropdownDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_C, Permissao.PDA_I, Permissao.PDA_A, Permissao.PDA_E, Policy = "Bearer")]
        public async Task<IActionResult> ListarMotivos([FromServices] IObterMotivosAusenciaUseCase useCase)
        {
            var motivsoAusencia = await useCase.Executar();            

            return Ok(motivsoAusencia);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir(long id, [FromServices] IExcluirAnotacaoFrequenciaAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(id));
        }
    }
}