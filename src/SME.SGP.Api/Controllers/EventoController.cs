using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

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
        //[Permissao(Permissao.C_I, Policy = "Bearer")]
        public async Task<IActionResult> Post([FromBody]EventoDto eventoDto)
        {
            await comandosEvento.Salvar(eventoDto);
            return Ok();
        }
    }
}