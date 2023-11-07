using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/consulta-criancas-estudantes-ausentes")]
    [Authorize("Bearer")]
    public class ConsultaCriancasEstudanteAusentesController : ControllerBase
    {
        [HttpGet("turma/alunos")]
        [ProducesResponseType(typeof(IEnumerable<AlunosAusentesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CCEA_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTurmasAlunosAusentes([FromQuery] FiltroObterAlunosAusentesDto filtro, [FromServices] IObterTurmasAlunosAusentesUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("ausencias")]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CCEA_NAAPA_C, Policy = "Bearer")]
        public IActionResult ObterAusencias()
        {
            var lista = EnumExtensao.ListarDto<EnumAusencias>().ToList().OrderBy(ausencia => ausencia.Id);

            return Ok(lista);
        }
    }
}
