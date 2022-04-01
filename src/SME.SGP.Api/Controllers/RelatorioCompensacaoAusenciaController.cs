using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;


namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/frequencias/compensacoes-ausencias")]
    [Authorize("Bearer")]
    public class RelatorioCompensacaoAusenciaController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RCA_C, Policy = "Bearer")]
        public async Task<IActionResult> Gerar(FiltroRelatorioCompensacaoAusenciaDto filtroRelatorioCompensacaoAusenciaDto, [FromServices] IRelatorioCompensacaoAusenciaUseCase relatorioCompensacaoAusenciaUseCase)
        {
            return Ok(await relatorioCompensacaoAusenciaUseCase.Executar(filtroRelatorioCompensacaoAusenciaDto));
        }
    }
}