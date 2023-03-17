using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;
using System;
using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;
using System.Linq;
using SME.SGP.Dominio;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/sondagem/analitico")]
    [Authorize("Bearer")]
    public class RelatorioAnaliticoSondagemController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RCA_C, Policy = "Bearer")]
        public async Task<IActionResult> Gerar(FiltroRelatorioAnaliticoSondagemDto filtroRelatorioAnaliticoSondagemDto, [FromServices] IRelatorioAnaliticoSondagemUseCase relatorioAnaliticoSondagemUseCase)
        {
            return Ok(await relatorioAnaliticoSondagemUseCase.Executar(filtroRelatorioAnaliticoSondagemDto));
        }

        [HttpGet("tiposondagem")]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RCA_C, Policy = "Bearer")]
        public IActionResult ObterSituacoes()
        {
            var lista = EnumExtensao.ListarDto<TipoSondagem>().ToList().OrderBy(tipo => tipo.Descricao);

            return Ok(lista);
        }
    }
}
