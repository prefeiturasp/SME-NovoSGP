using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        [Route("/api/v1/estudantes/autocomplete/ativos")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AlunoSimplesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterAlunosParAutoCompleteAtivos(FiltroBuscaEstudantesAtivoDto filtroBuscaAlunosDto, [FromServices] IObterAlunosAtivosPorUeENomeUseCase obterAlunosAtivosPorUeENomeUseCase)
        {
            return Ok(await obterAlunosAtivosPorUeENomeUseCase.Executar(filtroBuscaAlunosDto));
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

        [HttpGet("{codigoAluno}/foto")]
        [ProducesResponseType(typeof(ArquivoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterFotoAluno(string codigoAluno, [FromServices] IObterEstudanteFotoUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoAluno));
        }

        [HttpPost("{codigoAluno}/foto")]
        [ProducesResponseType(typeof(ArquivoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> SalvarFotoAluno(string codigoAluno, [FromForm] IFormFile file, [FromServices] ISalvarFotoEstudanteUseCase useCase)
        {
            if (file.Length > 0)
                return Ok(await useCase.Executar(new EstudanteFotoDto() { AlunoCodigo = codigoAluno, File = file }));

            return BadRequest();

        }

        [HttpDelete("{codigoAluno}/foto")]
        [ProducesResponseType(typeof(ArquivoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ExcluirFotoAluno(string codigoAluno, [FromServices] IExcluirEstudanteFotoUseCase useCase)
        {

            return Ok(await useCase.Executar(codigoAluno));

        }
    }
}