using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.MapeamentoEstudante;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/mapeamento-estudante")]
    [Authorize("Bearer")]
    public class MapeamentoEstudanteController : ControllerBase
    {
        [HttpGet("secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ME_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesMapeamentoEstudante([FromQuery] long? mapeamentoEstudanteId,
            [FromServices] IObterSecoesMapeamentoSecaoUseCase obterSecoesMapeamentoSecaoUseCase)
        {
            return Ok(await obterSecoesMapeamentoSecaoUseCase.Executar(mapeamentoEstudanteId));
        }

        [HttpGet("questionario/{questionarioId}")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ME_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionario(long questionarioId, [FromQuery] long? mapeamentoEstudanteId, [FromServices] IObterQuestionarioMapeamentoEstudanteUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, mapeamentoEstudanteId));
        }

        [HttpGet("aluno/{codigoAluno}/turma/{turmaId}/bimestre/{bimestre}/identificador")]
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ME_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterIdentificador(string codigoAluno, long turmaId, int bimestre, [FromServices] IObterIdentificadorMapeamentoEstudanteUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoAluno, turmaId, bimestre));
        }
    }
}
