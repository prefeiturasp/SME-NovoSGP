using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
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

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/dashboard/registro-itinerancia")]
    //[Authorize("Bearer")]
    public class DashboardRegistroItineranciaController : ControllerBase
    {
        [HttpGet("visitas-paais")]
        [ProducesResponseType(typeof(IEnumerable<DashboardItineranciaDto>), 200)]
        [ProducesResponseType(typeof(IEnumerable<DashboardItineranciaDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DRI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterVisitasPAAIs([FromQuery] int anoLetivo, [FromQuery] long dreId, [FromQuery] long ueId, [FromQuery] int mes, [FromServices] IObterDashboardItineranciaVisitasPAAIsUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroDashboardItineranciaDto()
            {
                AnoLetivo = anoLetivo,
                DreId = dreId,
                UeId = ueId,
                Mes = mes
            }));
        }

        [HttpGet("objetivos")]
        [ProducesResponseType(typeof(IEnumerable<DashboardItineranciaDto>), 200)]
        [ProducesResponseType(typeof(IEnumerable<DashboardItineranciaDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DRI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterObjetivos([FromQuery] int anoLetivo, [FromQuery] long dreId, [FromQuery] long ueId, [FromQuery] int mes, [FromQuery] string rf, [FromServices] IObterDashboardItineranciaObjetivosUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroDashboardItineranciaDto()
            {
                AnoLetivo = anoLetivo,
                DreId = dreId,
                UeId = ueId,
                Mes = mes,
                RF = rf
            }));
        }
    }
}