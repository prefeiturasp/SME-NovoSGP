using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/pendencias")]
    [Authorize("Bearer")]
    public class PendenciasController : ControllerBase
    {
        //[HttpGet()]
        //[ProducesResponseType(typeof(PaginacaoResultadoDto<PendenciaDto>), 200)]
        //[ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //public async Task<IActionResult> Listar([FromServices] IObterPendenciasUseCase useCase)
        //{
        //    return Ok(await useCase.Executar());
        //}

        [HttpGet()]
        [Route("turma/{turmaId}/tipo/{tipoPendencia}/titulo/{tituloPendencia}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<PendenciaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Listar(string turmaId, int? tipoPendencia, string tituloPendencia, 
                                                [FromServices] IObterPendenciasUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroPendenciasUsuarioDto(turmaId, tipoPendencia, tituloPendencia)));
        }
    }
}