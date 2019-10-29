using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
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

        [HttpGet]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Listar([FromQuery]FiltroEventosDto filtroEventosDto, [FromServices] IConsultasEvento consultasEvento)
        {
            return Ok(await consultasEvento.Listar(filtroEventosDto));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventoCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        //[Permissao(Permissao.C_I, Policy = "Bearer")]
        public IActionResult ObterPorId(long id, [FromServices] IConsultasEvento consultasEvento)
        {
            return Ok(consultasEvento.ObterPorId(id));
        }
    }
}