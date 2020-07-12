using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/componentes-curriculares")]
    [Authorize("Bearer")]
    public class ComponentesCurricularesController : ControllerBase
    {
        [HttpGet("modalidades/{modalidade}/anos/{anoLetivo}/anos-escolares")]
        [ProducesResponseType(typeof(IEnumerable<ComponenteCurricularEol>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterDresAtribuicoes(Modalidade modalidade, [FromQuery] string[] anosEscolares, int anoLetivo, [FromServices] IObterComponentesCurricularesPorAnoEscolarUseCase obterComponentesCurricularesPorAnoEscolarUseCase)
        {
            return Ok(await obterComponentesCurricularesPorAnoEscolarUseCase.Executar(modalidade, anoLetivo, anosEscolares));
        }
    }
}