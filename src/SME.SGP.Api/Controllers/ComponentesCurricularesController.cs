using Microsoft.AspNetCore.Mvc;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;
using SME.SGP.Aplicacao;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/componentes-curriculares")]
    public class ComponentesCurricularesController : ControllerBase
    {
        [HttpPost("ues/{ueId}/turmas")]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Obter(string ueId, string[] turmas, [FromServices] IObterComponentesCurricularesPorTurmaECodigoUeUseCase obterComponentesCurricularesPorTurmaECodigoUeUseCase)
        {
            var filtro = new FiltroComponentesCurricularesPorTurmaECodigoUeDto {CodigoUe = ueId, CodigosDeTurmas = turmas};
            return Ok(await obterComponentesCurricularesPorTurmaECodigoUeUseCase.Executar(filtro));
        }
    }
}