using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/historico-escolar")]
    public class HistoricoEscolarController : ControllerBase
    {

        [HttpPost]
        [Route("alunos")]
        [ProducesResponseType(typeof(IEnumerable<AlunoSimplesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async System.Threading.Tasks.Task<IActionResult> ObterAlunosAsync([FromBody] FiltroBuscaAlunosDto filtroBuscaAlunosDto, [FromServices] IObterListaAlunosFiltroHistoricoEscolarUseCase obterListaAlunosFiltroHistoricoEscolarUseCase)
        {
            return Ok(await obterListaAlunosFiltroHistoricoEscolarUseCase.Executar(filtroBuscaAlunosDto));
        }


        [HttpPost]
        [Route("gerar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public IActionResult Gerar(FiltroRelatorioHistoricoEscolarDto filtroRelatorioHistoricoEscolarDto)
        {
            return Ok(true);
        }

    }
}