using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/notas-conceitos-finais")]
    [Authorize("Bearer")]
    public class RelatorioNotasEConceitosFinaisController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RNCF_C,Policy = "Bearer")]
        public async Task<IActionResult> Gerar(FiltroRelatorioNotasEConceitosFinaisDto filtro, [FromServices] IRelatorioNotasEConceitosFinaisUseCase relatorioNotasEConceitosFinaisUseCase)
        {
            return Ok(await relatorioNotasEConceitosFinaisUseCase.Executar(filtro));
        }
    }
}