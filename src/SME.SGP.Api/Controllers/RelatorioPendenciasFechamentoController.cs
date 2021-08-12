using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/fechamentos/pendencias")]
    public class RelatorioPendenciasFechamentoController : ControllerBase
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
        [Route("tipo-pendencias")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public IActionResult ObterTipoPendencias()
        {
            var situacoes = Enum.GetValues(typeof(TipoPendencia))
                        .Cast<TipoPendencia>()
                        .Select(d => new { codigo = (int)d, descricao = d.Name(), grupo = d.GroupName() })
                        .ToList();
            return Ok(situacoes);
        }
    }
}