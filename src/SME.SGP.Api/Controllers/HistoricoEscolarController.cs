using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/historico-escolar")]
    public class HistoricoEscolarController : ControllerBase
    {
        [HttpPost]
        [Route("gerar")]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Gerar(FiltroHistoricoEscolarDto filtroHistoricoEscolarDto, [FromServices] IHistoricoEscolarUseCase historicoEscolarUseCase)
        {
            return Ok(await historicoEscolarUseCase.Executar(filtroHistoricoEscolarDto));
        }
    }
}