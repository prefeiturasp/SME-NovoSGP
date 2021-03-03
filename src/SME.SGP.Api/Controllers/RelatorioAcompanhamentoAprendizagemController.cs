using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/acompanhamentos-aprendizagem")]
    public class RelatorioAcompanhamentoAprendizagemController : ControllerBase
    {
        [HttpGet("{turmaId}/{semestre}/frequencias/{alunoCodigo}")]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterFrequenciaAluno(long turmaId, int semestre, string alunoCodigo, [FromServices] IRelatorioAcompanhamentoAprendizagemObterFrequenciaUseCase useCase)
        {
            return Ok(await useCase.Executar(turmaId, semestre, alunoCodigo));
        }
    }
}