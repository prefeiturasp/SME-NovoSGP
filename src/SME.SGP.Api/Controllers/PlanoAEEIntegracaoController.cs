using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PlanoAEE;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PlanoAEE;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/plano-aee/integracoes")]
    //[ChaveIntegracaoSgpApi]
    public class PlanoAEEIntegracaoController : ControllerBase
    {
        
        [HttpGet("turma/{codigoTurma}/existe")]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> VerificarExistenciaPlanoAEEPorTurma(
            long codigoTurma, 
            [FromQuery] string? codigoUe, 
            [FromServices] IVerificarExistenciaPlanoAEEPorTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroTurmaPlanoAEEDto(codigoTurma, codigoUe)));
        }
    }
}