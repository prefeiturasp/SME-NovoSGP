using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v1/dashboard/compensacoes/ausencia")]
    public class DashboardCompensacaoAusenciaController : Controller
    {
        [HttpGet("anos/{AnoLetivo}/dres/{dreId}/ues/{ueId}/modalidades/{modalidade}/consolidado/anos-turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(GraficoCompensacaoAusenciaDto), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTotalAusenciasCompensadas(int anoLetivo, long dreId, long ueId, int modalidade, int bimestre, [FromQuery] int semestre, [FromServices] IObterDadosDashboardTotalAusenciasCompensadasUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade, bimestre, semestre));
        }

        [HttpGet("anos/{AnoLetivo}/dres/{dreId}/ues/{ueId}/modalidades/{modalidade}/consolidado/compensacoes-consideradas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(GraficoCompensacaoAusenciaDto), 200)]
        [Permissao(Permissao.DF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTotalCompensacoesConsideradas(int anoLetivo, long dreId, long ueId, int modalidade, [FromQuery] int bimestre, int semestre, [FromServices] IObterDadosDashboardTotalAtividadesCompensacaoUseCase useCase)
        {
            return Ok(await useCase.Executar(anoLetivo, dreId, ueId, modalidade, bimestre, semestre));
        }
    }
}
