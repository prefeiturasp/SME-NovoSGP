using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/planos/aulas")]
    [ValidaDto]
    public class PlanoAulaController : ControllerBase
    {
        [HttpPost("migrar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Permissao.PA_A, Policy = "Bearer")]
        public async Task<IActionResult> Migrar(MigrarPlanoAulaDto migrarPlanoAulaDto, [FromServices]IComandosPlanoAula comandosPlanoAula)
        {
            await comandosPlanoAula.Migrar(migrarPlanoAulaDto);
            return Ok();
        }

        [HttpGet("{aulaId}")]
        [ProducesResponseType(typeof(PlanoAulaRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPlanoAula(long aulaId,
            [FromServices] IConsultasPlanoAula consultas)
        {
            // Data Escola Turma Dis
            var planoDto = await consultas.ObterPlanoAulaPorAula(aulaId);

            if (planoDto != null)
                return Ok(planoDto);
            else
                return StatusCode(204);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_I, Permissao.PDA_A, Policy = "Bearer")]
        public async Task<IActionResult> Post(PlanoAulaDto planoAulaDto, [FromServices]ISalvarPlanoAulaUseCase useCase)
        {
            //await comandos.Salvar(planoAulaDto);
            return Ok(await useCase.Executar(planoAulaDto));
        }

        [HttpPost("validar-existente")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public IActionResult ValidarPlanoAnualExistente(FiltroPlanoAulaExistenteDto filtroPlanoAulaExistenteDto, [FromServices]IConsultasPlanoAula consultasPlanoAula)
        {
            return Ok(consultasPlanoAula.ValidarPlanoAulaExistente(filtroPlanoAulaExistenteDto));
        }
    }
}