using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class EventosAulasCalendarioController : ControllerBase
    {
        private readonly IConsultasEventosAulasCalendario consultasEventosAulasCalendario;

        public EventosAulasCalendarioController(IConsultasEventosAulasCalendario consultasEventosAulasCalendario)
        {
            this.consultasEventosAulasCalendario = consultasEventosAulasCalendario ??
              throw new System.ArgumentNullException(nameof(consultasEventosAulasCalendario));
        }

        [HttpPost]
        [ProducesResponseType(typeof(EventosAulasCalendarioDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("meses/eventos-aulas")]
        [Permissao(Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEventosAulasMensais(FiltroEventosAulasCalendarioDto filtro)
        {
            var retorno = await consultasEventosAulasCalendario.ObterEventosAulasMensais(filtro);
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EventosAulasTipoCalendarioDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("meses/{mes}/tipos/eventos-aulas")]
        [Permissao(Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTipoEventosAulas(FiltroEventosAulasCalendarioMesDto filtro)
        {
            var retorno = await consultasEventosAulasCalendario.ObterTipoEventosAulas(filtro);
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }
    }
}