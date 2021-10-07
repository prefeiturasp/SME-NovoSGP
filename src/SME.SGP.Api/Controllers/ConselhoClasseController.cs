using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/conselhos-classe")]
    [Authorize("Bearer")]
    public class ConselhoClasseController : ControllerBase
    {
        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/turmas/{codigoTurma}/bimestres/{bimestre}/recomendacoes")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ConsultasConselhoClasseRecomendacaoConsultaDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRecomendacoesAlunoFamilia([FromServices] IConsultasConselhoClasseRecomendacao consultasConselhoClasseRecomendacao,
            long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre, [FromQuery]bool consideraHistorico = false)
        {
            var retorno = await consultasConselhoClasseRecomendacao.ObterRecomendacoesAlunoFamilia(conselhoClasseId, fechamentoTurmaId, alunoCodigo, codigoTurma, bimestre, consideraHistorico);
            return Ok(retorno);
        }

        [HttpPost("recomendacoes")]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(AuditoriaConselhoClasseAlunoDto), 200)]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> SalvarRecomendacoesAlunoFamilia(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto, [FromServices] IComandosConselhoClasseAluno comandosConselhoClasseAluno)
        {
            return Ok(await comandosConselhoClasseAluno.SalvarAsync(conselhoClasseAlunoDto));
        }

        [HttpPost("{conselhoClasseId}/notas/alunos/{codigoAluno}/turmas/{codigoTurma}/bimestres/{bimestre}/fechamento-turma/{fechamentoTurmaId}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> PersistirNotas([FromServices] IComandosConselhoClasseNota comandosConselhoClasseNota,
           [FromBody] ConselhoClasseNotaDto conselhoClasseNotaDto, string codigoAluno, long conselhoClasseId, long fechamentoTurmaId, string codigoTurma, int bimestre)
        {
            return Ok(await comandosConselhoClasseNota.SalvarAsync(conselhoClasseNotaDto, codigoAluno, conselhoClasseId, fechamentoTurmaId, codigoTurma, bimestre));
        }

        [HttpGet("detalhamento/{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult DetalhamentoNota(long id, [FromServices] IConsultasConselhoClasseNota consultasConselhoClasseNota)
        {
            return Ok(consultasConselhoClasseNota.ObterPorId(id));
        }

        [HttpGet("turmas/{turmaCodigo}/bimestres/{bimestre}/alunos/{alunoCodigo}/final/{ehFinal}/consideraHistorico/{consideraHistorico}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ConselhoClasseAlunoResumoDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterConselhoClasseTurma(string turmaCodigo, int bimestre, string alunoCodigo, bool ehFinal, bool consideraHistorico, [FromServices] IConsultasConselhoClasse consultasConselhoClasse)
            => Ok(await consultasConselhoClasse.ObterConselhoClasseTurma(turmaCodigo, alunoCodigo, bimestre, ehFinal, consideraHistorico));

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/turmas/{codigoTurma}/parecer")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ParecerConclusivoDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterParecerConclusivoAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, [FromServices] IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
            => Ok(await consultasConselhoClasseAluno.ObterParecerConclusivo(conselhoClasseId, fechamentoTurmaId, alunoCodigo, codigoTurma));

        [HttpGet("/turmas/{codigoTurma}/alunos/{alunoCodigo}/parecer")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ParecerConclusivoDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterParecerConclusivoPorTurmaAluno(string codigoTurma, string alunoCodigo, [FromServices] IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
            => Ok(await consultasConselhoClasseAluno.ObterParecerConclusivoAlunoTurma(codigoTurma, alunoCodigo));

        [HttpPost("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/parecer")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ParecerConclusivoDto), 200)]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> GerarParecerConclusivoAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, [FromServices] IComandosConselhoClasseAluno comandosConselhoClasseAluno)
            => Ok(await comandosConselhoClasseAluno.GerarParecerConclusivoAsync(conselhoClasseId, fechamentoTurmaId, alunoCodigo));

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/turmas/{codigoTurma}/bimestres/{bimestre}/sintese")]
        [ProducesResponseType(typeof(IEnumerable<ConselhoDeClasseGrupoMatrizDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSintesesConselhoDeClasse(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre, [FromServices] IConsultasConselhoClasseAluno consultasConselhoClasseAluno)
        {
            return Ok(await consultasConselhoClasseAluno.ObterListagemDeSinteses(conselhoClasseId, fechamentoTurmaId, alunoCodigo, codigoTurma, bimestre));
        }

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/turmas/{codigoTurma}/bimestres/{bimestre}/notas")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<ConselhoClasseAlunoNotasConceitosDto>), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterNotasAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre, [FromServices] IConsultasConselhoClasseAluno consultasConselhoClasseAluno, [FromQuery] bool consideraHistorico = false)
            => Ok(await consultasConselhoClasseAluno.ObterNotasFrequencia(conselhoClasseId, fechamentoTurmaId, alunoCodigo, codigoTurma, bimestre, consideraHistorico));

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/imprimir")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ImprimirConselhoTurma(long conselhoClasseId, long fechamentoTurmaId, [FromServices] IImpressaoConselhoClasseTurmaUseCase impressaoConselhoClasseTurmaUseCase)
          => Ok(await impressaoConselhoClasseTurmaUseCase.Executar(new FiltroRelatorioConselhoClasseAlunoDto() { ConselhoClasseId = conselhoClasseId, FechamentoTurmaId = fechamentoTurmaId }));

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/imprimir")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(bool), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ImprimirConselhoAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, [FromServices] IImpressaoConselhoClasseAlunoUseCase impressaoConselhoClasseAlunoUseCase)
          => Ok(await impressaoConselhoClasseAlunoUseCase.Executar(new FiltroRelatorioConselhoClasseAlunoDto() { ConselhoClasseId = conselhoClasseId, CodigoAluno = alunoCodigo, FechamentoTurmaId = fechamentoTurmaId }));

        [HttpGet("pareceres-conclusivos")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<ConselhoClasseParecerConclusivoDto>), 200)]
        [Permissao(Permissao.RPC_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarPareceresConclusivos([FromServices] IObterPareceresConclusivosUseCase obterPareceresConclusivosUseCase)
         => Ok(await obterPareceresConclusivosUseCase.Executar());


        [HttpGet("turmas/{turmaId}/bimestres")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(IEnumerable<BimestreComConselhoClasseTurmaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterBimestresComConselhoClasseTurma(long turmaId, [FromServices] IObterBimestresComConselhoClasseTurmaUseCase obterBimestresComConselhoClasseTurmaUseCase)
         => Ok(await obterBimestresComConselhoClasseTurmaUseCase.Executar(turmaId));
    }
}