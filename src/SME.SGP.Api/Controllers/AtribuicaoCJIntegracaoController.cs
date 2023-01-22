using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicoes/cjs/integracoes")]
    [ChaveIntegracaoSgpApi]
    [ValidaDto]
    public class AtribuicaoCJIntegracaoController : ControllerBase
    {
        [HttpGet("{ueId}/{anoLetivo}")]
        [ProducesResponseType(typeof(IEnumerable<AtribuicaoCJListaRetornoDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Get(string ueId,int anoLetivo, [FromServices] IListarAtribuicoesCJPorFiltroUseCase useCase)
        {
            return Ok(await useCase.Executar(new AtribuicaoCJListaFiltroDto{UeId = ueId,AnoLetivo = anoLetivo}));
        }

    }
}