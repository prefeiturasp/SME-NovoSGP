using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/planos/aulas")]
    [ValidaDto]
    public class PlanoAulaController : ControllerBase
    {
        [HttpGet("{turmaId}/disciplina/{disciplinaId}")]
        [ProducesResponseType(typeof(PlanoAulaRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.PDA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPlanoAula([FromQuery] DateTime data, int turmaId, string disciplinaId,
            [FromServices] IConsultasPlanoAula consultas)
        {
            // Data Escola Turma Dis
            var planoDto = await consultas.ObterPlanoAulaPorTurmaDisciplina(data, turmaId, disciplinaId);

            if (planoDto != null)
                return Ok(planoDto);
            else
                return StatusCode(204);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_I, Permissao.PDA_A, Policy = "Bearer")]
        public async Task<IActionResult> Post(PlanoAulaDto planoAulaDto, [FromServices]IComandosPlanoAula comandos)
        {
            await comandos.Salvar(planoAulaDto);
            return Ok();
        }
    }
}
