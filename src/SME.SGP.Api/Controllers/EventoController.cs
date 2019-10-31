using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
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
        [ProducesResponseType(typeof(IEnumerable<CalendarioEventosMesesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        //[Permissao(Permissao.C_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterMeses([FromServices] IConsultasEvento consultasEvento,
                            [FromQuery]CalendarioEventosMesesFiltroDto calendarioEventoMesesFiltro)

        {
            var retorno = await consultasEvento.ObterQuantidadeDeEventosPorMeses(calendarioEventoMesesFiltro);
            if (retorno.Count() > 0)
                return Ok(retorno);
            else return StatusCode(204);
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