using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
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
        [ProducesResponseType(typeof(DiaEventoAula), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("meses/dias/eventos-aulas")]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEventoAulasDia(FiltroEventosAulasCalendarioDiaDto filtro)
        {
            var retorno = await consultasEventosAulasCalendario.ObterEventoAulasDia(filtro);

            return Ok(retorno);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EventosAulasCalendarioDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("meses/eventos-aulas")]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        
        public async Task<IActionResult> ObterEventosAulasMensais(FiltroEventosAulasCalendarioDto filtro)
        {
            var retorno = await consultasEventosAulasCalendario.ObterEventosAulasMensais(filtro);

            if (!retorno.Any())
                return NoContent();

            return Ok(retorno);
        }
        [HttpGet]
        [ProducesResponseType(typeof(EventoAulaDiaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("{tipoCalendarioId}/meses/{mes}/eventos-aulas")]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEventosAulasMensaisPorCalendario(long tipoCalendarioId, int mes, [FromQuery]FiltroAulasEventosCalendarioDto filtro, 
            [FromServices]IMediator mediator, [FromServices]IServicoUsuario  servicoUsuario)
        {
            return Ok(await ObterAulasEventosProfessorCalendarioPorMesUseCase.Executar(mediator, filtro, tipoCalendarioId, mes, servicoUsuario));            
        }
        [HttpGet]
        [ProducesResponseType(typeof(EventosAulasNoDiaCalendarioDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("{tipoCalendarioId}/meses/{mes}/dias/{dia}/eventos-aulas")]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEventosAulasNoDiaPorCalendario(long tipoCalendarioId, int mes, int dia, 
            [FromQuery]FiltroAulasEventosCalendarioDto filtro, [FromServices]IMediator mediator, [FromServices]IServicoUsuario servicoUsuario)
        {            
            var retorno = await ObterAulasEventosProfessorCalendarioPorMesDiaUseCase.Executar(mediator, filtro, tipoCalendarioId, mes, dia, filtro.AnoLetivo, servicoUsuario);
            retorno.EventosAulasMes = await ObterAulasEventosProfessorCalendarioPorMesUseCase.Executar(mediator, filtro, tipoCalendarioId, mes, servicoUsuario);
            return Ok(retorno);
        }
        [HttpPost]
        [ProducesResponseType(typeof(EventosAulasTipoCalendarioDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("meses/tipos/eventos-aulas")]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTipoEventosAulas(FiltroEventosAulasCalendarioMesDto filtro)
        {
            var retorno = await consultasEventosAulasCalendario.ObterTipoEventosAulas(filtro);

            if (!retorno.Any())
                return StatusCode(204);

            return Ok(retorno);
        }
    }
}