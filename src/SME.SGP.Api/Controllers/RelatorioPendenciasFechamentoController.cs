using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;


namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorio/pendencias-fechamento")]
    public class RelatorioPendenciasFechamentoController : ControllerBase
    {
        [HttpPost]
        [Route("gerar")]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Gerar(FiltroRelatorioPendenciasFechamentoDto filtroRelatorioPendenciasFechamentoDto, [FromServices] IRelatorioPendenciasFechamentoUseCase relatorioPendenciasFechamentoUseCase)
        {
            return Ok(await relatorioPendenciasFechamentoUseCase.Executar(filtroRelatorioPendenciasFechamentoDto));
        }
    }
}