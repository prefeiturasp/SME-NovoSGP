using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/turma/integracoes/")]
    [ChaveIntegracaoSgpApi]
    [ValidaDto]
    public class TurmaIntegracaoController : ControllerBase
    {
        [HttpGet("modalidades")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(IEnumerable<TurmaModalidadeCodigoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterModalidades([FromQuery] string[] turmasCodigo, [FromServices] IObterTurmaModalidadesPorCodigosUseCase obterTurmaModalidadesPorCodigos)
        {
            return Ok(await obterTurmaModalidadesPorCodigos.Executar(turmasCodigo));
        }
    }
}
