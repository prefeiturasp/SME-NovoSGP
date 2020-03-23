using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/fechamentos/pendencias")]
    [ValidaDto]
    public class PendenciasFechamentosController : ControllerBase
    {
        [HttpGet("listar")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<PendenciaFechamentoResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.PF_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar([FromQuery]FiltroPendenciasFechamentosDto filtro, [FromServices]IConsultasPendenciaFechamento consultasPendenciaFechamento)
        {
            return Ok(await consultasPendenciaFechamento.Listar(filtro));
        }
    }
}
