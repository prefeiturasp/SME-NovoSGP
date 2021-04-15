using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/funcionarios")]
    [Authorize("Bearer")]
    public class FuncionarioController : ControllerBase
    {
        [HttpPost]
        [Route("pesquisa")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<UsuarioEolRetornoDto>), 200)]
        //[Permissao(Permissao.AS_C, Policy = "Bearer")]
        public async Task<IActionResult> PesquisaFuncionariosPorDreUe([FromBody] FiltroPesquisaFuncionarioDto filtro, [FromServices] IPesquisaFuncionariosPorDreUeUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet]
        [Route("dres/{dreId}/paais")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<UsuarioEolRetornoDto>), 200)]
        public async Task<IActionResult> ObterFuncionariosPAAIs(long dreId, [FromServices] IObterFuncionariosPAAIPorDreUseCase useCase)
        {
            return Ok(await useCase.Executar(dreId));
        }

    }
}