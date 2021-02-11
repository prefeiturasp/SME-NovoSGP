using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.Relatorios;
using SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlunoDto = SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar.AlunoDto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/estudante")]
    public class EstudanteController : ControllerBase
    {

        [HttpPost]
        [Route("pesquisa")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AlunoSimplesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterAlunos(FiltroBuscaEstudanteDto filtroBuscaAlunosDto, [FromServices] IObterAlunosPorCodigoEolNomeUseCase obterAlunosPorCodigoEolNomeUseCase)
        {
            return Ok(await obterAlunosPorCodigoEolNomeUseCase.Executar(filtroBuscaAlunosDto));
        }
                
        [HttpGet("informacoes-escolares")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterInformacoesEscolaresDoAluno([FromQuery] string codigoAluno, [FromQuery] string codigoTurma, [FromServices] IObterInformacoesEscolaresDoAlunoUseCase ObterInformacoesEscolaresDoAlunoUseCase)
        {
            return Ok(await ObterInformacoesEscolaresDoAlunoUseCase.Executar(codigoAluno, codigoTurma));
        }

        [HttpGet("{codigoAluno}/anosLetivos/{anoLetivo}")]
        [ProducesResponseType(typeof(AlunoReduzidoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterAlunosPorCodigo(string codigoAluno, int anoLetivo, [FromServices] IObterAlunoPorCodigoEolEAnoLetivoUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoAluno, anoLetivo));
        }
    }
}