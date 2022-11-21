using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;
using System.Linq;


namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/encaminhamento-naapa")]
    [Authorize("Bearer")]
    public class EncaminhamentoNAAPAController : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        // [Permissao(Permissao.NAAPA_C, Policy = "Bearer")] 
        public async Task<IActionResult> ObterEncaminhamentosNAAPA([FromQuery] FiltroEncaminhamentoNAAPADto filtro,
            [FromServices] IObterEncaminhamentoNAAPAUseCase useCase)
        {
            // filtro = new FiltroEncaminhamentoNAAPADto()
            // {
            //     ExibirHistorico = true,
            //     AnoLetivo = 2022,
            //     DreId = 7,
            //     CodigoUe = "307288",
            //     TurmaId = 1549249,
            //     //DataAberturaQueixaInicio = new DateTime(2022, 11, 1),
            //     //DataAberturaQueixaFim = new DateTime(2022, 11, 18),
            //     Situacao = 1,
            //     Prioridade = 1,
            // };

            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesPorEtapaDeEncaminhamentoNAAPA([FromQuery] long encaminhamentoAeeId, [FromServices] IObterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase obterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase)
        {
            return Ok(await obterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase.Executar(encaminhamentoAeeId));
        }

        [HttpGet("questionario")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionario([FromQuery] long questionarioId, [FromQuery] long? encaminhamentoId, [FromQuery] string codigoAluno, [FromQuery] string codigoTurma, [FromServices] IObterQuestionarioEncaminhamentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, encaminhamentoId, codigoAluno, codigoTurma));
        }

        [HttpGet("situacoes")]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.DNA_C, Policy = "Bearer")]
        public IActionResult ObterSituacoes()
        {
            var lista = EnumExtensao.ListarDto<SituacaoNAAPA>().ToList().OrderBy(situacao => situacao.Descricao);

            return Ok(lista);
        }

        [HttpGet("prioridades")]
        [ProducesResponseType(typeof(IEnumerable<PrioridadeEncaminhamentoNAAPADto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.DNA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPrioridades([FromServices] IObterPrioridadeEncaminhamentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }
    }
}
