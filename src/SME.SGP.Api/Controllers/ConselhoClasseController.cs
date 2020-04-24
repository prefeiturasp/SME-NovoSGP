using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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
        [ProducesResponseType(typeof(ParecerConclusivoDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterParecerConclusivoAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, [FromServices]IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
            => Ok(await consultasConselhoClasseAluno.ObterParecerConclusivo(conselhoClasseId, fechamentoTurmaId, alunoCodigo));



        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/sintese")]
        [ProducesResponseType(typeof(ConselhoDeClasseGrupoMatrizDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSintesesConselhoDeClasse(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            var retorno = new List<ConselhoDeClasseGrupoMatrizDto>
            {
                new ConselhoDeClasseGrupoMatrizDto
                {
                    Titulo = "Enriquecimento curricular",
                    Id = 1,
                    ComponenteSinteses = new List<ConselhoDeClasseComponenteSinteseDto>
                    {
                        new ConselhoDeClasseComponenteSinteseDto
                        {
                            Codigo = "1",
                            Nome = "Matematica",
                            ParecerFinal = "Frequente",
                            PercentualFrequencia = 100,
                            TotalFaltas = 0
                        },
                        new ConselhoDeClasseComponenteSinteseDto
                        {
                            Codigo = "2",
                            Nome = "Portugues",
                            ParecerFinal = "Frequente",
                            PercentualFrequencia = 75,
                            TotalFaltas = 4
                        },
                        new ConselhoDeClasseComponenteSinteseDto
                        {
                            Codigo = "3",
                            Nome = "Geografia",
                            ParecerFinal = "Não Frequente",
                            PercentualFrequencia = 15,
                            TotalFaltas = 10
                        },
                    }
                },
                new ConselhoDeClasseGrupoMatrizDto
                {
                    Titulo = "Diversificada",
                    Id = 2,
                    ComponenteSinteses = new List<ConselhoDeClasseComponenteSinteseDto>
                    {
                        new ConselhoDeClasseComponenteSinteseDto
                        {
                            Codigo = "1",
                            Nome = "Matematica",
                            ParecerFinal = "Frequente",
                            PercentualFrequencia = 100,
                            TotalFaltas = 0
                        },
                        new ConselhoDeClasseComponenteSinteseDto
                        {
                            Codigo = "2",
                            Nome = "Portugues",
                            ParecerFinal = "Frequente",
                            PercentualFrequencia = 75,
                            TotalFaltas = 4
                        },
                        new ConselhoDeClasseComponenteSinteseDto
                        {
                            Codigo = "3",
                            Nome = "Geografia",
                            ParecerFinal = "Não Frequente",
                            PercentualFrequencia = 15,
                            TotalFaltas = 10
                        },
                    }
                },
                 new ConselhoDeClasseGrupoMatrizDto
                {
                    Titulo = "Integral",
                    Id = 3,
                    ComponenteSinteses = new List<ConselhoDeClasseComponenteSinteseDto>
                    {
                        new ConselhoDeClasseComponenteSinteseDto
                        {
                            Codigo = "1",
                            Nome = "Matematica",
                            ParecerFinal = "Frequente",
                            PercentualFrequencia = 100,
                            TotalFaltas = 0
                        },
                        new ConselhoDeClasseComponenteSinteseDto
                        {
                            Codigo = "2",
                            Nome = "Portugues",
                            ParecerFinal = "Frequente",
                            PercentualFrequencia = 75,
                            TotalFaltas = 4
                        },
                        new ConselhoDeClasseComponenteSinteseDto
                        {
                            Codigo = "3",
                            Nome = "Geografia",
                            ParecerFinal = "Não Frequente",
                            PercentualFrequencia = 15,
                            TotalFaltas = 10
                        },
                    }
                }
            };

            return Ok(retorno);
        }

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/notas")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ConselhoClasseAlunoNotasConceitosDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterNotasAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, [FromServices]IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
            => Ok(await consultasConselhoClasseAluno.ObterNotasFrequencia(conselhoClasseId, fechamentoTurmaId, alunoCodigo));
    }
}