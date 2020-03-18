using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/planos/anual")]
    [ValidaDto]
    public class PlanoAnualController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PlanoAnualCompletoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> Get(string turmaId, string ueId, int anoLetivo, long componenteCurricularEolId, [FromServices]IConsultasPlanoAnual consultasPlanoAnual)
        {
            return Ok(await consultasPlanoAnual.ObterPorUETurmaAnoEComponenteCurricular(ueId, turmaId, anoLetivo, componenteCurricularEolId));
        }

        [HttpPost("migrar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Permissao.PA_A, Policy = "Bearer")]
        public async Task<IActionResult> Migrar(MigrarPlanoAnualDto migrarPlanoAnualDto, [FromServices]IComandosPlanoAnual comandosPlanoAnual)
        {
            await comandosPlanoAnual.Migrar(migrarPlanoAnualDto);
            return Ok();
        }

        [HttpPost("obter")]
        [ProducesResponseType(typeof(PlanoAnualCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter(FiltroPlanoAnualDto filtroPlanoAnualDto, [FromServices]IConsultasPlanoAnual consultasPlanoAnual)
        {
            return Ok(await consultasPlanoAnual.ObterPorEscolaTurmaAnoEBimestre(filtroPlanoAnualDto));
        }

        [HttpPost("obter/expandido")]
        [ProducesResponseType(typeof(PlanoAnualCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterExpandido(FiltroPlanoAnualBimestreExpandidoDto filtroPlanoAnualDto, [FromServices]IConsultasPlanoAnual consultasPlanoAnual)
        {
            return Ok(await consultasPlanoAnual.ObterBimestreExpandido(filtroPlanoAnualDto));
        }

        [HttpGet("objetivos")]
        [ProducesResponseType(typeof(PlanoAnualCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(204)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterObjetivos(FiltroPlanoAnualDisciplinaDto filtro, [FromServices]IConsultasPlanoAnual consultasPlanoAnual)
        {
            var objetivosPlano = await consultasPlanoAnual.ObterObjetivosEscolaTurmaDisciplina(filtro);
            if (objetivosPlano != null)
                return Ok(objetivosPlano);
            else
                return StatusCode(204);
        }

        [HttpGet("turmas/copia")]
        [ProducesResponseType(typeof(PlanoAnualCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTurmasParaCopia([FromQuery] int turmaId, [FromQuery] long componenteCurricular, [FromServices]IConsultasPlanoAnual consultasPlanoAnual)
        {
            return Ok(await consultasPlanoAnual.ObterTurmasParaCopia(turmaId, componenteCurricular));
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<PlanoAnualCompletoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Permissao.PA_A, Policy = "Bearer")]
        public async Task<IActionResult> Post(PlanoAnualDto planoAnualDto, [FromServices]IComandosPlanoAnual comandosPlanoAnual)
        {            
            return Ok(await comandosPlanoAnual.Salvar(planoAnualDto));
        }

        [HttpPost("validar-existente")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public IActionResult ValidarPlanoAnualExistente(FiltroPlanoAnualDto filtroPlanoAnualDto, [FromServices]IConsultasPlanoAnual consultasPlanoAnual)
        {
            return Ok(consultasPlanoAnual.ValidarPlanoAnualExistente(filtroPlanoAnualDto));
        }
    }
}