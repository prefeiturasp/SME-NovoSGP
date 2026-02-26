using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Relatorios.SolicitacaoRelatorio;
using SME.SGP.Dominio.Dtos;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/solicitacao-relatorio")]
    [ApiController]
    [Authorize("Bearer")]
    public class SolicitacaoRelatorioController : ControllerBase
    {
        [HttpPost()]
        [ProducesResponseType(typeof(ImportacaoLogRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.IE_I_P_I, Policy = "Bearer")]
        /// <summary>
        /// Verifica se existe alguma solicitação de relatório pendente para o usuário logado
        /// </summary>
        public async Task<IActionResult> ObterSolicitacaoRelatorio([FromBody] FiltroSolicitacaoRelatorioDto filtro, [FromServices] IObterSolicitacaoRelatorioUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpPut()]
        [ProducesResponseType(typeof(ImportacaoLogRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.IE_I_P_I, Policy = "Bearer")]
        public async Task<IActionResult> AtualizarSolicitacaoRelatorio([FromBody] SolicitacaoRelatorioDto solicitacaoRelatorio, [FromServices] IRegistrarSolicitacaoRelatorioUseCase useCase)
        {
            await useCase.Executar(solicitacaoRelatorio);
            return Ok();
        }
    }
}
