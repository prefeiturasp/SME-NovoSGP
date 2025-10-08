using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlunoDto = SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar.AlunoDto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/estudante")]
    [Authorize("Bearer")]
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

        [HttpGet("{codigoAluno}/informacoes")]
        public async Task<IActionResult> ObterInformacoesAlunoPorCodigo(string codigoAluno, [FromServices] IObterInformacoesAlunoPorCodigoUseCase ObterInformacoesAlunoPorCodigoUseCase)
        {
            return Ok(await ObterInformacoesAlunoPorCodigoUseCase.Executar(codigoAluno));
        }

        [HttpGet("{codigoAluno}/anosLetivos/{AnoLetivo}")]
        [ProducesResponseType(typeof(AlunoReduzidoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterAlunosPorCodigo(string codigoAluno, int anoLetivo, string codigoTurma, [FromServices] IObterAlunoPorCodigoEolEAnoLetivoUseCase useCase, bool carregarDadosResponsaveis = false)
        {
            return Ok(await useCase.Executar(codigoAluno, anoLetivo, codigoTurma, carregarDadosResponsaveis));
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

        [HttpGet]
        [Route("graus-parentesco")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public IActionResult ObterSituacoes()
        {
            var grausParentesco = Enum.GetValues(typeof(GrauParentesco))
                        .Cast<GrauParentesco>()
                        .Select(d => new { codigo = (int)d, descricao = d.Name() })
                        .ToList();
            return Ok(grausParentesco);
        }
        [HttpGet]
        [Route("turmas-programa")]
        [ProducesResponseType(typeof(IEnumerable<AlunoTurmaProgramaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTurmasProgramaAluno([FromQuery]string codigoAluno, [FromQuery] int? anoLetivo, [FromServices] IObterEstudanteTurmasProgramaUseCase useCase, [FromQuery] bool filtrarSituacaoMatricula = true)
        {
            return Ok(await useCase.Executar(codigoAluno, anoLetivo, filtrarSituacaoMatricula));
        }      
    }
}