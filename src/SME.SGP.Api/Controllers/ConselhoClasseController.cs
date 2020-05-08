using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(AuditoriaConselhoClasseAlunoDto), 200)]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> SalvarRecomendacoesAlunoFamilia(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto, [FromServices]IComandosConselhoClasseAluno comandosConselhoClasseAluno)
        {
            return Ok(await comandosConselhoClasseAluno.SalvarAsync(conselhoClasseAlunoDto));
        }

        [HttpPost("{conselhoClasseId}/notas/alunos/{codigoAluno}/fechamento-turma/{fechamentoTurmaId}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> PersistirNotas([FromServices]IComandosConselhoClasseNota comandosConselhoClasseNota,
           [FromBody]ConselhoClasseNotaDto conselhoClasseNotaDto, string codigoAluno, long conselhoClasseId, long fechamentoTurmaId)
        {
            return Ok(await comandosConselhoClasseNota.SalvarAsync(conselhoClasseNotaDto, codigoAluno, conselhoClasseId, fechamentoTurmaId));
        }

        [HttpGet("detalhamento/{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)] 
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult DetalhamentoNota(long id, [FromServices] IConsultasConselhoClasseNota consultasConselhoClasseNota)
        {            
            return Ok(consultasConselhoClasseNota.ObterPorId(id));
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
        [ProducesResponseType(typeof(ParecerConclusivoDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterParecerConclusivoAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, [FromServices]IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
            => Ok(await consultasConselhoClasseAluno.ObterParecerConclusivo(conselhoClasseId, fechamentoTurmaId, alunoCodigo));

        [HttpPost("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/parecer")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ParecerConclusivoDto), 200)]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> GerarParecerConclusivoAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, [FromServices]IComandosConselhoClasseAluno comandosConselhoClasseAluno)
            => Ok(await comandosConselhoClasseAluno.GerarParecerConclusivoAsync(conselhoClasseId, fechamentoTurmaId, alunoCodigo));

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/sintese")]
        [ProducesResponseType(typeof(IEnumerable<ConselhoDeClasseGrupoMatrizDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSintesesConselhoDeClasse(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, [FromServices]IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
        {
            return Ok(await consultasConselhoClasseAluno.ObterListagemDeSinteses(conselhoClasseId, fechamentoTurmaId, alunoCodigo));
        }

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/notas")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<ConselhoClasseAlunoNotasConceitosDto>), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterNotasAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, [FromServices]IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
            => Ok(await consultasConselhoClasseAluno.ObterNotasFrequencia(conselhoClasseId, fechamentoTurmaId, alunoCodigo));

        [HttpGet("turmas/{turmaCodigo}/impressao")]
    [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<ConselhoClasseAlunoNotasConceitosDto>), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ImprimirTurma(long conselhoClasseId, [FromServices]IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
          => Ok(await consultasConselhoClasseAluno.ObterNotasFrequencia(conselhoClasseId, fechamentoTurmaId, alunoCodigo));

        [HttpGet("turmas/{turmaCodigo}/alunos/{alunoCodigo}/impressao")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<ConselhoClasseAlunoNotasConceitosDto>), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ImprimirAluno(long conselhoClasseId, [FromServices]IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
         => Ok(await consultasConselhoClasseAluno.ObterNotasFrequencia(conselhoClasseId, fechamentoTurmaId, alunoCodigo));
    }
}