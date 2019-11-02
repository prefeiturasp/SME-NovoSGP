using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class DiasLetivosCalendarioController : ControllerBase
    {
        private readonly IComandosDiasLetivos comandos;

        public DiasLetivosCalendarioController(IComandosDiasLetivos comandos)
        {
            this.comandos = comandos ?? throw new System.ArgumentNullException(nameof(comandos));
        }

        [HttpGet]
        [ProducesResponseType(typeof(DiasLetivosDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("{tipoCalendarioId}/dias-letivos")]
        //[Permissao(Permissao.C_C, Policy = "Bearer")]
        public IActionResult CalcularDiasLetivos(long tipoCalendarioId)
        {
            return Ok(new DiasLetivosDto { DiasLetivos = 200, EstaAbaixoPermitido = false });
        }
    }
}