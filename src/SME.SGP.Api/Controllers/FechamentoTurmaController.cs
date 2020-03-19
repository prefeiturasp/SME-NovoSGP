using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/fechamentos/turmas")]
    public class FechamentoTurmaController : ControllerBase
    {
        [HttpPost()]
        [ProducesResponseType(typeof(AuditoriaFechamentoTurmaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] IEnumerable<FechamentoTurmaDisciplinaDto> fechamentoTurma, [FromServices] IComandosFechamentoTurmaDisciplina comandos)
            => Ok(await comandos.Salvar(fechamentoTurma));

        [HttpGet()]
        [ProducesResponseType(typeof(FechamentoTurmaDisciplinaBimestreDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar(string turmaCodigo, long disciplinaCodigo, int? bimestre, [FromServices] IConsultasFechamentoTurmaDisciplina consultas)
            => Ok(await consultas.ObterNotasFechamentoTurmaDisciplina(turmaCodigo, disciplinaCodigo, bimestre));

        [HttpPost("anotacoes/alunos/")]
        [ProducesResponseType(typeof(AuditoriaFechamentoTurmaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> SalvarAnotacao([FromBody] AnotacaoAlunoDto anotacaoAluno, [FromServices] IComandosFechamentoTurmaDisciplina comandos)
            => Ok(await comandos.SalvarAnotacaoAluno(anotacaoAluno));

        [HttpGet("anotacoes/alunos/{codigoAluno}/fechamentos/{fechamentoId}/turmas/{codigoTurma}/anos/{anoLetivo}")]
        [ProducesResponseType(typeof(AnotacaoAlunoCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.FB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAnotacaoAluno(string codigoAluno, long fechamentoId, string codigoTurma, int anoLetivo, [FromServices] IConsultasFechamentoTurmaDisciplina consultas)
           => Ok(await consultas.ObterAnotacaoAluno(codigoAluno, fechamentoId, codigoTurma, anoLetivo));

    }
}