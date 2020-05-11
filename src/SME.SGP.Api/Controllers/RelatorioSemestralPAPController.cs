using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpGet("semestres")]
        [ProducesResponseType(typeof(IEnumerable<SemestreAcompanhamentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.NC_C, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterSemestres()
        {
            return Ok(new List<SemestreAcompanhamentoDto>()
            {
                new SemestreAcompanhamentoDto(1, "Acompanhamento 1º Semestre"),
                new SemestreAcompanhamentoDto(2, "Acompanhamento 2º Semestre")
            });
        }

        [HttpGet("turmas/{turmaCodigo}/semestres/{semestre}/alunos/{alunoCodigo}")]
        [ProducesResponseType(typeof(RelatorioSemestralAlunoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.NC_C, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterRelatorioAluno(string alunoCodigo, string turmaCodigo, int semestre, [FromServices]IConsultasRelatorioSemestralAluno consultasRelatorioSemestralAluno)
        {
            return Ok(new RelatorioSemestralAlunoDto()
            {
                PodeEditar = true,
                RelatorioSemestralId = 1,
                RelatorioSemestralAlunoId = 1,
                Secoes = new List<RelatorioSemestralAlunoSecaoDto>()
                {
                    new RelatorioSemestralAlunoSecaoDto(1, "Histórico do Estudante", "Trajetória do estudante, reprovações, histórico de faltas, acompanhamento das aprendizagens", true, "Teste valor histórico"),
                    new RelatorioSemestralAlunoSecaoDto(2, "Dificuldades", "Dificuldades apresentadas inicialmente ", true, "Teste valor dificuldades"),
                    new RelatorioSemestralAlunoSecaoDto(3, "Encaminhamentos", "Encaminhamentos realizados", true, "Teste valor Encaminhamentos"),
                    new RelatorioSemestralAlunoSecaoDto(4, "Avanços", "Avanços observados", true, "Teste valor Avanços"),
                    new RelatorioSemestralAlunoSecaoDto(5, "Outros", "Outras observações", false, "Teste valor outros"),
                },
                Auditoria = new AuditoriaDto()
                {
                    Id = 1,
                    CriadoPor = "Fulano",
                    CriadoEm = DateTime.Today,
                    CriadoRF = "789456123",
                    AlteradoPor = "Fulano",
                    AlteradoEm = DateTime.Now,
                    AlteradoRF = "789456123",
                }
            });
        }

        [HttpPost("turmas/{turmaCodigo}/semestres/{semestre}/alunos/{alunoCodigo}")]
        [ProducesResponseType(typeof(AuditoriaRelatorioSemestralAlunoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.NC_C, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> SalvarRelatorioAluno(string alunoCodigo, string turmaCodigo, int semestre
            , [FromBody] RelatorioSemestralAlunoPersistenciaDto relatorioSemestralAlunoDto
            , [FromServices]IComandosRelatorioSemestralAluno comandosRelatorioSemestralAluno)
            => Ok(await comandosRelatorioSemestralAluno.Salvar(alunoCodigo, turmaCodigo, semestre, relatorioSemestralAlunoDto));

        [HttpGet("turmas/{turmaCodigo}/alunos/anos/{anoLetivo}/semestres/{semestre}")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDadosBasicosRelatorioPAPDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterRelatorioAluno(string turmaCodigo, int anoLetivo, int semestre, [FromServices]IConsultasRelatorioSemestralAluno consultasRelatorioSemestralAluno)
            => Ok(await consultasRelatorioSemestralAluno.ObterListaAlunosAsync(turmaCodigo, anoLetivo, semestre));
    }
}