using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.Comandos;
using SME.SGP.Aplicacao.Interfaces.Consultas;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/evento/tipo")]
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
            var eventoTipoDto = consultasEventoTipo.ObtenhaPorId(codigo);

            if (eventoTipoDto == null)
                NoContent();

            return Ok(eventoTipoDto);
        }

        [HttpPost("Listar")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IList<EventoTipoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Listar([FromBody]FiltroEventoTipoDto filtroEventoTipoDto, [FromServices]IConsultasEventoTipo consultasEventoTipo)
        {
            var listaEventoTipo = consultasEventoTipo.Listar(filtroEventoTipoDto);

            if (listaEventoTipo == null)
                return NoContent();

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