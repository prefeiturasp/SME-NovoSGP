using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/eventos")]
    [Authorize("Bearer")]
    public class EventoController : ControllerBase
    {
        private readonly IComandosEvento comandosEvento;

        public EventoController(IComandosEvento comandosEvento)
        {
            this.comandosEvento = comandosEvento ?? throw new System.ArgumentNullException(nameof(comandosEvento));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public IActionResult Post([FromBody]EventoDto eventoDto)
        {
            comandosEvento.Salvar(eventoDto);
            return Ok();
        }
    }
}