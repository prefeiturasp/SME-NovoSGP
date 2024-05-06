using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos.Relatorios.MapeamentoEstudantes;
using SME.SGP.Aplicacao.Interfaces;
using System;


namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorio-mapeamento-estudantes")]
    [Authorize("Bearer")]
    public class RelatorioMapeamentoEstudantesController : ControllerBase
    {
        [HttpGet("filtros/opcoes-resposta")]
        [ProducesResponseType(typeof(OpcoesRespostaFiltroRelatorioMapeamentoEstudanteDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RME_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFiltrosOpcoesRespostaMapeamentoEstudante(
                                            [FromServices] IObterFiltrosOpcoesRespostaRelatorioMapeamentoUseCase useCase)
        => Ok(await useCase.Executar());
    }
}
