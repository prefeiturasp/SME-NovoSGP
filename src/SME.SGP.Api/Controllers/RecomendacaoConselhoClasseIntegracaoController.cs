using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/conselhos-classe/recomendacoes/integracoes")]
    [ChaveIntegracaoSgpApi]
    public class RecomendacaoConselhoClasseIntegracaoController : ControllerBase
    {

        [HttpGet("alunos")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterRecomendascoesAlunoTurma([FromQuery] FiltroRecomendacaoConselhoClasseAlunoTurmaDto filtro, 
                                                                        [FromServices] IObterRecomendacoesPorAlunoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }

}