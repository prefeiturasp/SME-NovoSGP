using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/conselhos-classe")]
    [Authorize("Bearer")]
    public class ConselhoClasseController : ControllerBase
    {
        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/recomendacoes")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ConsultasConselhoClasseRecomendacaoConsultaDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRecomendacoesAlunoFamilia([FromServices]IConsultasConselhoClasseRecomendacao consultasConselhoClasseRecomendacao,
            long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            var retorno = await consultasConselhoClasseRecomendacao.ObterRecomendacoesAlunoFamilia(conselhoClasseId, fechamentoTurmaId, alunoCodigo);
            return Ok(retorno);
        }

        [HttpPost("recomendacoes")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(AuditoriaConselhoClasseAlunoDto), 200)]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> SalvarRecomendacoesAlunoFamilia(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto, [FromServices]IComandosConselhoClasseAluno comandosConselhoClasseAluno)
        {
            return Ok(await comandosConselhoClasseAluno.SalvarAsync(conselhoClasseAlunoDto));
        }

        [HttpGet("turmas/{turmaCodigo}/bimestres/{bimestre}/alunos/{alunoCodigo}/final/{ehFinal}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ConselhoClasseAlunoResumoDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterConselhoClasseTurma(string turmaCodigo, int bimestre, string alunoCodigo, bool ehFinal, [FromServices]IConsultasConselhoClasse consultasConselhoClasse)
            => Ok(await consultasConselhoClasse.ObterConselhoClasseTurma(turmaCodigo, alunoCodigo, bimestre, ehFinal));

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/parecer")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ConselhoClasseAlunoResumoDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterParecerConclusivoAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, [FromServices]IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
            => Ok(await consultasConselhoClasseAluno.ObterParecerConclusivo(conselhoClasseId, fechamentoTurmaId, alunoCodigo));
    }
}