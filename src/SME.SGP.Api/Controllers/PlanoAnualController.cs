using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/planos/anual")]
    [ValidaDto]
    public class PlanoAnualController : ControllerBase
    {
        private readonly IComandosPlanoAnual comandosPlanoAnual;

        public PlanoAnualController(IComandosPlanoAnual comandosPlanoAnual)
        {
            this.comandosPlanoAnual = comandosPlanoAnual ?? throw new System.ArgumentNullException(nameof(comandosPlanoAnual));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Post(PlanoAnualDto planoAnualDto)
        {
            comandosPlanoAnual.Salvar(planoAnualDto);
            return Ok();
        }
    }
}