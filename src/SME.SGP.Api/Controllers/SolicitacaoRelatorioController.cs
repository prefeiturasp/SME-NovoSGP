using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Relatorios.SolicitacaoRelatorio;
using SME.SGP.Dominio.Dtos;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/solicitacao-relatorio")]
    [ApiController]
    [ChaveIntegracaoSgpApi]
    public class SolicitacaoRelatorioController : ControllerBase
    {
        [HttpPost()]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        /// <summary>
        /// Verifica se existe alguma solicitação de relatório pendente para o usuário logado
        /// </summary>
        public async Task<IActionResult> ObterSolicitacaoRelatorio([FromBody] FiltroSolicitacaoRelatorioDto filtro, [FromServices] IObterSolicitacaoRelatorioUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpPut()]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> AtualizarSolicitacaoRelatorio([FromBody] SolicitacaoRelatorioDto solicitacaoRelatorio, [FromServices] IRegistrarSolicitacaoRelatorioUseCase useCase)
        {
            await useCase.Executar(solicitacaoRelatorio);
            return Ok();
        }
    }
}
