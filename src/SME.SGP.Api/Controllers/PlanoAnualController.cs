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
        [HttpGet]
        [ProducesResponseType(typeof(PlanoAnualCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Get(FiltroPlanoAnualDto filtroPlanoAnualDto, [FromServices]IConsultasPlanoAnual consultasPlanoAnual)
        {
            return Ok(await consultasPlanoAnual.ObterPorEscolaTurmaAnoEBimestre(filtroPlanoAnualDto));
        }

        [HttpPost("migrar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Migrar(MigrarPlanoAnualDto migrarPlanoAnualDto, [FromServices]IComandosPlanoAnual comandosPlanoAnual)
        {
            await comandosPlanoAnual.Migrar(migrarPlanoAnualDto);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Post(PlanoAnualDto planoAnualDto, [FromServices]IComandosPlanoAnual comandosPlanoAnual)
        {
            comandosPlanoAnual.Salvar(planoAnualDto);
            return Ok();
        }

        [HttpGet("validar-existente")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult ValidarPlanoAnualExistente(FiltroPlanoAnualDto filtroPlanoAnualDto, [FromServices]IConsultasPlanoAnual consultasPlanoAnual)
        {
            return Ok(consultasPlanoAnual.ValidarPlanoAnualExistente(filtroPlanoAnualDto));
        }
    }
}