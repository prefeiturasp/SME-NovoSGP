using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/novo-encaminhamento-naapa")]
    //[Authorize("Bearer")]
    public class NovoEncaminhamentoNAAPAController : ControllerBase
    {
        [HttpGet("secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesDeEncaminhamento([FromQuery] long? ecaminhamentoNaapaId,
           [FromServices] IObterSecoesEncaminhamentoIndividualNAAPAUseCase obterSecoesDeEncaminhamentoNAAPAUseCase)
        {
            return Ok(await obterSecoesDeEncaminhamentoNAAPAUseCase.Executar(ecaminhamentoNaapaId));
        }

        [HttpGet("questionario")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionario([FromQuery] long questionarioId, [FromQuery] long? encaminhamentoId, [FromQuery] string codigoAluno, [FromQuery] string codigoTurma, [FromServices] IObterQuestionarioEncaminhamentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, encaminhamentoId, codigoAluno, codigoTurma));
        }

        [HttpGet("aluno/{codigoAluno}/existe-encaminhamento-ativo")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ExisteEncaminhamentoAtivoParaAluno(string codigoAluno, [FromServices] IExisteEncaminhamentoNAAPAAtivoParaAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoAluno));
        }

        [HttpGet("obterEncaminhamentoPorTipo")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEncaminhamentosPaginados([FromQuery] FiltroNovoEncaminhamentoNAAPADto filtro,
            [FromServices] IObterNovosEncaminhamentosNAAPAPorTipoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }
}