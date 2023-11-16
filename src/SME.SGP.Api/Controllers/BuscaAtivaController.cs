using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/busca-ativa")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class BuscaAtivaController : ControllerBase
    {
        [HttpGet("registros-acao/secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CCEA_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesDeRegistroAcao([FromQuery] FiltroSecoesDeRegistroAcao filtro,
            [FromServices] IObterSecoesRegistroAcaoSecaoUseCase obterSecoesRegistroAcaoSecaoUseCase)
        {
            return Ok(await obterSecoesRegistroAcaoSecaoUseCase.Executar(filtro));
        }

    }
}