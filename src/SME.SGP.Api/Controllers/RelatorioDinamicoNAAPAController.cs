using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorio-dinamico-naapa")]
    [Authorize("Bearer")]
    public class RelatorioDinamicoNAAPAController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(RelatorioDinamicoNAAPADto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RDNAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEncaminhamentosNAAPA(
                                            [FromQuery] FiltroRelatorioDinamicoNAAPADto filtro,
                                            [FromServices] IRelatorioDinamicoObterEncaminhamentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("dinamico")] 
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RDNAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestoesPorModalidadesParaRelatorioDinamico(
                                            [FromQuery] int? modalidadeId,
                                            [FromServices] IObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesUseCase useCase)
        {
            return Ok(await useCase.Executar(modalidadeId));
        }
    }
}
