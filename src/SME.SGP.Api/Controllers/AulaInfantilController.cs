using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/aulas_infantil")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class AulaInfantilController : Controller
    {
        private readonly IMediator mediator;

        public AulaInfantilController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("sincronizar-aulas")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SincronizarAulasTurma([FromQuery] long? codigoTurma)
        {
            var dados = new DadosCriacaoAulasAutomaticasCarregamentoDto() { CodigoTurma = codigoTurma?.ToString() };
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaSincronizarAulasInfantil, dados, Guid.NewGuid(), null));
            return Ok();
        }
    }
}
