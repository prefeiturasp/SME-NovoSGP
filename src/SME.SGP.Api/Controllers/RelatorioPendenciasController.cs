using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/fechamentos/pendencias")]
    public class RelatorioPendenciasController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Gerar(FiltroRelatorioPendenciasFechamentoDto filtroRelatorioPendenciasFechamentoDto, [FromServices] IRelatorioPendenciasFechamentoUseCase relatorioPendenciasFechamentoUseCase)
        {
            return Ok(await relatorioPendenciasFechamentoUseCase.Executar(filtroRelatorioPendenciasFechamentoDto));
        }

        [HttpGet]
        [Route("tipos")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public IActionResult ObterTipoPendencias([FromQuery] bool opcaoTodos, [FromServices] IRelatorioPendenciasFechamentoUseCase relatorioPendenciasFechamentoUseCase)
        {

            return Ok(relatorioPendenciasFechamentoUseCase.ListarTodosTipos(opcaoTodos));
        }
    }
}