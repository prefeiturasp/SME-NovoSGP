using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using SME.SGP.Infra.Dtos.Relatorios;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Api.Middlewares;
using SME.SGP.Dominio;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/conselhos-classe/recomendacoes/integracoes")]
    [Authorize("Bearer")]
    public class RecomendacaoConselhoClasseIntegracaoController : ControllerBase
    {

        [HttpGet("alunos")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterRecomendascoesAlunoTurma([FromQuery] FiltroRecomendacaoConselhoClasseAlunoTurmaDto filtro, 
                                                                        [FromServices] IObterRecomendacoesPorAlunoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }

}