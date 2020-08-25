using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/recuperacao-paralela")]
    public class RelatorioRecuperacaoParalelaController : ControllerBase
    {
        [HttpPost]        
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Gerar(FiltroRelatorioRecuperacaoParalelaDto filtro, [FromServices] IRelatorioRecuperacaoParalelaUseCase relatorioRecuperacaoParalelaUseCase)
        {
            return Ok(await relatorioRecuperacaoParalelaUseCase.Executar(filtro));
        }
    }


}