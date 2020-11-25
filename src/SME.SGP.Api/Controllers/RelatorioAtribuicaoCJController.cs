using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/atribuicoes/cjs")]
    public class RelatorioAtribuicaoCJController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Gerar(FiltroRelatorioAtribuicaoCJDto filtros, [FromServices] IRelatorioAtribuicaoCJUseCase relatorioAtribuicaoCJUseCase)
        {
            return Ok(await relatorioAtribuicaoCJUseCase.Executar(filtros));
        }
    }
}
