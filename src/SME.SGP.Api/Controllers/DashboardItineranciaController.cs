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

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/dashboard/itinerancia")]
    //[Authorize("Bearer")]
    public class DashboardItineranciaController : ControllerBase
    {
        [HttpGet("visitas-paais")]
        [ProducesResponseType(typeof(IEnumerable<ItineranciaVisitaDto>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ItineranciaVisitaDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterVisitasPAAIs([FromQuery] int anoLetivo, [FromQuery] long dreId, [FromQuery] long ueId, [FromQuery] int mes, [FromServices] IObterVisitasPAAIsUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroDashboardItineranciaDto()
            {
                AnoLetivo = anoLetivo,
                DreId = dreId,
                UeId = ueId,
                Mes = mes
            }));
        }
    }
}