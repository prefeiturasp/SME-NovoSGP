using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Anos;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadoEvento;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/comunicadoevento")]
    [Authorize("Bearer")]
    public class ComunicadoEventoController : ControllerBase
    {
        [HttpPost("ListarPorCalendario")]
        [ProducesResponseType(typeof(IEnumerable<ListarEventosPorCalendarioRetornoDto>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ListarEventosPorCalendarioRetornoDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ListarEventosPorCalendario([FromBody] ListarEventoPorCalendarioDto listarEventoPorCalendario, [FromServices] IListarEventosPorCalendarioUseCase listarEventosPorCalendarioUseCase)
        {
            var retorno = await listarEventosPorCalendarioUseCase.Executar(
                listarEventoPorCalendario.TipoCalendario,
                listarEventoPorCalendario.AnoLetivo,
                listarEventoPorCalendario.CodigoDre,
                listarEventoPorCalendario.CodigoUe,
                listarEventoPorCalendario.Modalidade
                );

            if (retorno == null || !retorno.Any())
                return NoContent();

            return Ok(retorno);
        }
    }
}