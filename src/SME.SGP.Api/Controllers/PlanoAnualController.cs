using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Threading.Tasks;

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

        [HttpGet]
        [ProducesResponseType(typeof(PlanoAnualCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Get(FiltroPlanoAnualDto filtroPlanoAnualDto, [FromServices]IConsultasPlanoAnual consultasPlanoAnual)
        {
            return Ok(await consultasPlanoAnual.ObterPorEscolaTurmaAnoEBimestre(filtroPlanoAnualDto));
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