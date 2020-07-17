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
        [HttpPost]
        [Route("obter")]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Obter(FiltroComponentesCurricularesPorTurmaECodigoUeDto filtroComponentesCurricularesPorTurmaECodigoUeDto, [FromServices] IObterComponentesCurricularesPorTurmaECodigoUeUseCase obterComponentesCurricularesPorTurmaECodigoUeUseCase)
        {
            return Ok(await obterComponentesCurricularesPorTurmaECodigoUeUseCase.Executar(filtroComponentesCurricularesPorTurmaECodigoUeDto));
        }


    }
}