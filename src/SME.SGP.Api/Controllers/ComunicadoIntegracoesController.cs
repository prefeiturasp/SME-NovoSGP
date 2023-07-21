using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/comunicados/integracoes")]
    [ApiController]
    [ChaveIntegracaoSgpApi]
    public class ComunicadoIntegracoesController : ControllerBase
    {
        [HttpGet("ano-atual")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTodosAnoAtual([FromServices] IObterComunicadosAnoAtualUseCase useCase)
        {
            var comunicados = await useCase.Executar();
            return Ok(comunicados);
        }
    }
}
