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
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/encaminhamento-naapa")]
    [Authorize("Bearer")]
    public class EncaminhamentoNAAPAController : ControllerBase
    {

        [HttpPost("salvar")]
        [ProducesResponseType(typeof(IEnumerable<ResultadoEncaminhamentoNAAPADto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_I, Policy = "Bearer")]
        public async Task<IActionResult> RegistrarEncaminhamento([FromBody] EncaminhamentoNAAPADto encaminhamentoNAAPADto, [FromServices] IRegistrarEncaminhamentoNAAPAUseCase registrarEncaminhamentoNAAPAUseCase)
        {
            return Ok(await registrarEncaminhamentoNAAPAUseCase.Executar(encaminhamentoNAAPADto));
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")] 
        public async Task<IActionResult> ObterEncaminhamentosNAAPA([FromQuery] FiltroEncaminhamentoNAAPADto filtro,
            [FromServices] IObterEncaminhamentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesPorEtapaDeEncaminhamentoNAAPA([FromQuery] long encaminhamentoNAAPAId, [FromServices] IObterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase obterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase)
        {
            return Ok(await obterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase.Executar(encaminhamentoNAAPAId));
        }

        [HttpGet("questionario")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionario([FromQuery] long questionarioId, [FromQuery] long? encaminhamentoId, [FromQuery] string codigoAluno, [FromQuery] string codigoTurma, [FromServices] IObterQuestionarioEncaminhamentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, encaminhamentoId, codigoAluno, codigoTurma));
        }

        [HttpGet("situacoes")]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public IActionResult ObterSituacoes()
        {
            var lista = EnumExtensao.ListarDto<SituacaoNAAPA>().ToList().OrderBy(situacao => situacao.Descricao);

            return Ok(lista);
        }

        [HttpGet("prioridades")]
        [ProducesResponseType(typeof(IEnumerable<PrioridadeEncaminhamentoNAAPADto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPrioridades([FromServices] IObterPrioridadeEncaminhamentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }
        
        [HttpGet("{encaminhamentoId}")]
        [ProducesResponseType(typeof(EncaminhamentoNAAPARespostaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEncaminhamento(long encaminhamentoId, [FromServices] IObterEncaminhamentoNAAPAPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoId));
        }
    }
}
