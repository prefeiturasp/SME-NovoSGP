using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/pap/semestral")]
    [Authorize("Bearer")]
    public class RelatorioSemestralPAPController : ControllerBase
    {
        [HttpGet("semestres/{turmaCodigo}")]
        [ProducesResponseType(typeof(IEnumerable<SemestreAcompanhamentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.NC_C, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterSemestres(string turmaCodigo, [FromServices]IMediator mediator)
        {
            var retorno = await ObterListaSemestresUseCase.Executar(mediator, turmaCodigo);

            if (retorno == null || !retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("turmas/{turmaCodigo}/semestres/{semestre}/alunos/{alunoCodigo}")]
        [ProducesResponseType(typeof(RelatorioSemestralAlunoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.NC_C, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterRelatorioAluno(string alunoCodigo, string turmaCodigo, int semestre, [FromServices]IMediator mediator)
        {
            return Ok(await ObterRelatorioSemestralPorTurmaSemestreAlunoUseCase.Executar(mediator, alunoCodigo, turmaCodigo, semestre));
        }

        [HttpPost("turmas/{turmaCodigo}/semestres/{semestre}/alunos/{alunoCodigo}")]
        [ProducesResponseType(typeof(AuditoriaRelatorioSemestralAlunoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.NC_C, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> SalvarRelatorioAluno(string alunoCodigo, string turmaCodigo, int semestre
            , [FromBody] RelatorioSemestralAlunoPersistenciaDto relatorioSemestralAlunoDto
            , [FromServices]IComandosRelatorioSemestralPAPAluno comandosRelatorioSemestralAluno)
            => Ok(await comandosRelatorioSemestralAluno.Salvar(alunoCodigo, turmaCodigo, semestre, relatorioSemestralAlunoDto));

        [HttpGet("turmas/{turmaCodigo}/alunos/anos/{anoLetivo}/semestres/{semestre}")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDadosBasicosDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterListaAlunos(string turmaCodigo, int anoLetivo, int semestre, [FromServices]IConsultasRelatorioSemestralPAPAluno consultasRelatorioSemestralAluno)
            => Ok(await consultasRelatorioSemestralAluno.ObterListaAlunosAsync(turmaCodigo, anoLetivo, semestre));
    }
}