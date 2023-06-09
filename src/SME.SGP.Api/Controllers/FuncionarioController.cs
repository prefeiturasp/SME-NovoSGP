using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Aplicacao;

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
        [Permissao(Permissao.AS_C,Permissao.OCO_C,Policy = "Bearer")]
        public async Task<IActionResult> PesquisaFuncionariosPorDreUe([FromBody] FiltroPesquisaFuncionarioDto filtro, [FromServices] IPesquisaFuncionariosPorDreUeUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet]
        [Route("dres/{dreId}/paais")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<UsuarioEolRetornoDto>), 200)]
        [Permissao(Permissao.AS_C, Permissao.RERI_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFuncionariosPAAIs(long dreId, [FromServices] IObterFuncionariosPAAIPorDreUseCase useCase)
        {
            return Ok(await useCase.Executar(dreId));
        }

        [HttpGet]
        [Route("codigoUe/{codigoUe}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<UsuarioEolRetornoDto>), 200)]
        [Permissao(Permissao.OCO_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFuncionariosPorUe(string codigoUe, string filtro,[FromServices] IObterFuncionariosPorUeUseCase useCase)
        {
            var consulta = await useCase.Executar(codigoUe, filtro);
            if (!consulta.Any())
                return StatusCode(204);
            return Ok(consulta);
        }
    }
}