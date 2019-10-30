using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/eventos")]
    [Authorize("Bearer")]
    public class EventoController : ControllerBase
    {
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Alterar(long id, [FromBody]EventoDto eventoDto, [FromServices]IComandosEvento comandosEvento)
        {
            await comandosEvento.Alterar(id, eventoDto);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Criar([FromServices]IComandosEvento comandosEvento, [FromBody]EventoDto eventoDto)
        {
            await comandosEvento.Criar(eventoDto);
            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        //[Permissao(Permissao.C_I, Policy = "Bearer")]
        public IActionResult Excluir(long[] eventosId, [FromServices]IComandosEvento comandosEvento)
        {
            comandosEvento.Excluir(eventosId);
            return Ok();
        }

        [HttpGet("meses")]
        [ProducesResponseType(typeof(IEnumerable<CalendarioEventosMeses>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        //[Permissao(Permissao.C_I, Policy = "Bearer")]
        public IActionResult ObterMeses([FromQuery]CalendarioEventosMesesFiltro calendarioEventoMesesFiltro)
        {
            var retornoMockado = new List<CalendarioEventosMeses>();

            retornoMockado.Add(new CalendarioEventosMeses() { Eventos = 2, Mes = 1 });
            retornoMockado.Add(new CalendarioEventosMeses() { Eventos = 3, Mes = 2 });
            retornoMockado.Add(new CalendarioEventosMeses() { Eventos = 5, Mes = 5 });
            retornoMockado.Add(new CalendarioEventosMeses() { Eventos = 1, Mes = 8 });
            retornoMockado.Add(new CalendarioEventosMeses() { Eventos = 3, Mes = 12 });

            return Ok(retornoMockado);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventoObterParaEdicaoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        //[Permissao(Permissao.C_I, Policy = "Bearer")]
        public IActionResult ObterPorId(long id, [FromServices] IConsultasEvento consultasEvento)
        {
            return Ok(consultasEvento.ObterPorId(id));
        }
    }
}