using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/processos/executando")]
    [Authorize("Bearer")]
    public class ProcessoExecutandoController : ControllerBase
    {
        [HttpGet("frequencias/turma/{turmaId}/disciplina/{disciplinaId}/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterCalculoFrequencia(string turmaId, string disciplinaId, int bimestre, [FromServices] IConsultasProcessoExecutando consultas)
            => Ok(await consultas.ExecutandoCalculoFrequencia(turmaId, disciplinaId, bimestre));
    }
}