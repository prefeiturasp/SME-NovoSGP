using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

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
        //[Permissao(Permissao.C_C, Policy = "Bearer")]
        [AllowAnonymous]//mudar
        public IActionResult ObterEventosAulasMensais(FiltroEventosAulasCalendarioDto filtro)
        {
            return Ok(consultasEventosAulasCalendario.ObterEventosAulasMensais(filtro));
        }

        [HttpPost]
        [ProducesResponseType(typeof(EventosAulasTipoCalendarioDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("meses/{mes}/tipos/eventos-aulas")]
        //[Permissao(Permissao.C_C, Policy = "Bearer")]
        [AllowAnonymous]//mudar
        public IActionResult ObterTipoEventosAulas(FiltroEventosAulasCalendarioMesDto filtro)
        {
            return Ok(consultasEventosAulasCalendario.ObterTipoEventosAulas(filtro));
        }
    }
}