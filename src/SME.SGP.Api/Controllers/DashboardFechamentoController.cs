using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidade;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.EscolaAqui;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos.DashboardFechamento;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/dashboard/fechamentos")]
    //[Authorize("Bearer")]
    public class DashboardFechamentoController : ControllerBase
    {
        [HttpGet("situacoes")]
        [ProducesResponseType(typeof(FechamentoSituacaoDto), 200)]
        [ProducesResponseType(typeof(FechamentoSituacaoDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSituacoesFechamento(
            [FromQuery] FiltroDashboardFechamentoDto filtroDashboardFechamentoDto,
            [FromServices] IObterFechamentoSituacaoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtroDashboardFechamentoDto));
        }
    }
}