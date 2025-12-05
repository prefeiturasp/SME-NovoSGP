using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Turma;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/pap")]
    [Authorize("Bearer")]
    public class RelatorioPAPController : ControllerBase
    {
        [HttpPost("salvar")]
        [ProducesResponseType(typeof(IEnumerable<ResultadoRelatorioPAPDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RPAP_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] RelatorioPAPDto relatorioPAPDto, [FromServices] ISalvarRelatorioPAPUseCase useCase)
        {
            return Ok(await useCase.Executar(relatorioPAPDto));
        }

        [HttpPost("copiar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RPAP_I, Policy = "Bearer")]
        public async Task<IActionResult> Copiar([FromBody] CopiarPapDto copiarPapDto,[FromServices] ICopiarRelatorioPAPUseCase useCase)
        {
            return Ok(await useCase.Executar(copiarPapDto));
        }

        [HttpGet("periodos/{codigoTurma}")]
        [ProducesResponseType(typeof(IEnumerable<PeriodosPAPDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RPAP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPeriodos(string codigoTurma, [FromServices] IObterPeriodosPAPUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoTurma));
        }

        [HttpGet("turma/{codigoTurma}/aluno/{codigoAluno}/periodo/{periodoIdPAP}/secoes")]
        [ProducesResponseType(typeof(SecaoTurmaAlunoPAPDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RPAP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoes(string codigoTurma, string codigoAluno, long periodoIdPAP, [FromServices] IObterSecoesPAPUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroObterSecoesDto(codigoTurma, codigoAluno, periodoIdPAP)));
        }

        [HttpGet("turma/{codigoTurma}/aluno/{codigoAluno}/periodo/{periodoIdPAP}/questionario/{questionarioId}")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RPAP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionario(string codigoTurma, string codigoAluno, long periodoIdPAP, long questionarioId, [FromQuery] long? papSecaoId, [FromServices] IObterQuestionarioPAPUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoTurma, codigoAluno, periodoIdPAP, questionarioId, papSecaoId));
        }

        [HttpGet("turma/{turmaCodigo}/relatorio-periodo/{periodoRelatorioPAPId}/alunos")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDadosBasicosDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RPAP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAlunos(string turmaCodigo, long periodoRelatorioPAPId, [FromServices] IObterAlunosPorPeriodoPAPUseCase useCase)
        {
            return Ok(await useCase.Executar(turmaCodigo, periodoRelatorioPAPId));
        }

        [HttpDelete("arquivo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RPAP_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirArquivo([FromQuery] Guid arquivoCodigo, [FromServices] IExcluirArquivoPAPUseCase useCase)
        {
            return Ok(await useCase.Executar(arquivoCodigo));
        }

        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RPAP_I, Policy = "Bearer")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromServices] IUploadDeArquivoUseCase useCase)
        {
            if (file.Length > 0)
                return Ok(await useCase.Executar(file, Dominio.TipoArquivo.AtendimentoNAAPA));

            return BadRequest();
        }

        [HttpGet("turmas-pap/{anoLetivo}/ues/{codigoUe}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTurmasPapPorAnoLetivo([FromRoute] long anoLetivo, string codigoUe,[FromServices] IObterTurmasPapPorAnoLetivoUseCase usecase)
        {
            return Ok(await usecase.Executar(anoLetivo,codigoUe));
        }
    }
}
