using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.MapeamentoEstudante;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using SME.SGP.Infra.Dtos.Sondagem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/mapeamentos-estudantes")]
    [Authorize("Bearer")]
    public class MapeamentoEstudanteController : ControllerBase
    {

        public MapeamentoEstudanteController()
        {
        }


        [HttpPost()]
        [ProducesResponseType(typeof(IEnumerable<ResultadoMapeamentoEstudanteDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ME_I, Permissao.ME_A, Policy = "Bearer")]
        public async Task<IActionResult> RegistrarMapeamentoEstudante([FromBody] MapeamentoEstudanteDto mapeamentoEstudante, [FromServices] IRegistrarMapeamentoEstudanteUseCase useCase)
        {
            return Ok(await useCase.Executar(mapeamentoEstudante));
        }

        [HttpGet("secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ME_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesMapeamentoEstudante([FromQuery] long? mapeamentoEstudanteId,
            [FromServices] IObterSecoesMapeamentoSecaoUseCase obterSecoesMapeamentoSecaoUseCase)
        {
            return Ok(await obterSecoesMapeamentoSecaoUseCase.Executar(mapeamentoEstudanteId));
        }

        [HttpGet("questionarios/{questionarioId}/questoes")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ME_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionario(long questionarioId, [FromQuery] FiltroQuestoesQuestionarioMapeamentoEstudanteDto filtro, 
                                                           [FromServices] IObterQuestionarioMapeamentoEstudanteUseCase useCase)
        {
            filtro.QuestionarioId = questionarioId;
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("alunos/{codigoAluno}/turmas/{turmaId}/bimestres/{bimestre}/identificador")]
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ME_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterIdentificador(string codigoAluno, long turmaId, int bimestre, [FromServices] IObterIdentificadorMapeamentoEstudanteUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoAluno, turmaId, bimestre));
        }
    }
}
