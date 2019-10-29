using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Alterar([FromServices]IComandosEvento comandosEvento, [FromQuery]long id, [FromBody]EventoDto eventoDto)
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
    }
}