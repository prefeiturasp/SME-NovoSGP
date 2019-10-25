using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.Comandos;
using SME.SGP.Aplicacao.Interfaces.Consultas;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/eventos/tipos")]
    [ValidaDto]
    public class EventoTipoController : ControllerBase
    {
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Permissao.PA_A, Policy = "Bearer")]
        public IActionResult Delete([FromBody]IEnumerable<long> codigos, [FromServices]IComandosEventoTipo comandosEventoTipo)
        {
            comandosEventoTipo.Remover(codigos);
            return Ok();
        }

        [HttpGet("{codigo}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(EventoTipoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Get(long codigo, [FromServices]IConsultasEventoTipo consultasEventoTipo)
        {
            var eventoTipoDto = consultasEventoTipo.ObterPorId(codigo);

            if (eventoTipoDto == null)
                NoContent();

            return Ok(eventoTipoDto);
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<EventoTipoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Listar([FromQuery]FiltroEventoTipoDto filtroEventoTipoDto, [FromServices]IConsultasEventoTipo consultasEventoTipo)
        {
            var listaEventoTipo = await consultasEventoTipo.Listar(filtroEventoTipoDto);

            return Ok(listaEventoTipo);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Permissao.PA_A, Policy = "Bearer")]
        public IActionResult Post([FromBody]EventoTipoDto eventoTipo, [FromServices]IComandosEventoTipo comandosEventoTipo)
        {
            comandosEventoTipo.Salvar(eventoTipo);
            return Ok();
        }
    }
}