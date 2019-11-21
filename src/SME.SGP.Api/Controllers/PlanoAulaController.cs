using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/planos/aula")]
    [ValidaDto]
    public class PlanoAulaController : ControllerBase
    {
        [HttpGet("obter")]
        [ProducesResponseType(typeof(PlanoAulaRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPlanoAula(FiltroPlanoAulaDto filtro, 
            [FromServices] IConsultasPlanoAula consultas)
        {
            var planoDto = await consultas.ObterPlanoAulaPorTurmaDisciplina(filtro.Data, filtro.TurmaId, filtro.DisciplinaId);

            if (planoDto != null)
                return Ok(planoDto);
            else
                return StatusCode(204);
        }



        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.PA_I, Permissao.PA_A, Policy = "Bearer")]
        public IActionResult Post(PlanoAulaDto planoAulaDto, [FromServices]IComandosPlanoAula comandos)
        {
            comandos.Salvar(planoAulaDto);
            return Ok();
        }
    }
}
