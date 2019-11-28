using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/eventos/tipos")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class EventoTipoController : ControllerBase
    {
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TE_A, Policy = "Bearer")]
        public IActionResult Alterar([FromServices]IComandosEventoTipo comandosEventoTipo,
            long id,
            [FromBody]EventoTipoInclusaoDto eventoTipo)
        {
            comandosEventoTipo.Alterar(eventoTipo, id);
            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TE_E, Policy = "Bearer")]
        public IActionResult Delete([FromBody]IEnumerable<long> codigos, [FromServices]IComandosEventoTipo comandosEventoTipo)
        {
            comandosEventoTipo.Remover(codigos);
            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(EventoTipoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TE_C, Permissao.E_C, Policy = "Bearer")]
        public IActionResult Get(long id, [FromServices]IConsultasEventoTipo consultasEventoTipo)
        {
            var eventoTipoDto = consultasEventoTipo.ObterPorId(id);

            if (eventoTipoDto == null)
                NoContent();

            return Ok(eventoTipoDto);
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<EventoTipoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TE_C, Permissao.E_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar([FromQuery]FiltroEventoTipoDto filtroEventoTipoDto, [FromServices]IConsultasEventoTipo consultasEventoTipo)
        {
            var listaEventoTipo = await consultasEventoTipo.Listar(filtroEventoTipoDto);

            return Ok(listaEventoTipo);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TE_I, Policy = "Bearer")]
        public IActionResult Post([FromBody]EventoTipoInclusaoDto eventoTipo, [FromServices]IComandosEventoTipo comandosEventoTipo)
        {
            comandosEventoTipo.Salvar(eventoTipo);
            return Ok();
        }
    }
}