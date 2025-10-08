using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicoes/cjs/integracoes")]
    [ChaveIntegracaoSgpApi]
    public class AtribuicaoCJIntegracaoController : ControllerBase
    {
        [HttpGet("escola/{ueCodigo}/AnoLetivo/{AnoLetivo}")]
        [ProducesResponseType(typeof(IEnumerable<AtribuicaoCJListaRetornoDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Get(string ueCodigo,int anoLetivo, [FromServices] IListarAtribuicoesCJPorFiltroUseCase useCase)
        {
            var retorno =  (await useCase.Executar(new AtribuicaoCJListaFiltroDto{UeId = ueCodigo,AnoLetivo = anoLetivo}));
            if (!retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

    }
}