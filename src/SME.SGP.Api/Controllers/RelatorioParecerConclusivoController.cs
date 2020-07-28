using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/pareceres-conclusivos")]
    public class RelatorioParecerConclusivoController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Gerar(FiltroRelatorioParecerConclusivoDto filtroRelatorioParecerConclusivoDto, [FromServices] IRelatorioParecerConclusivoUseCase relatorioParecerConclusivoUseCase)
        {
            return Ok(await relatorioParecerConclusivoUseCase.Executar(filtroRelatorioParecerConclusivoDto));
        }
    }
}