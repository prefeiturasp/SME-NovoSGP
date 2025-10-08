using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using SME.SGP.Infra.Dtos.Questionario;
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
        public async Task<IActionResult> ObterRecomendacoesAlunoFamilia([FromServices] IConsultaConselhoClasseRecomendacaoUseCase consultaConselhoClasseRecomendacaoUseCase,
            long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre, bool consideraHistorico = false)
        {
            var retorno = await consultaConselhoClasseRecomendacaoUseCase.Executar(
                new ConselhoClasseRecomendacaoDto()
                {
                    ConselhoClasseId = conselhoClasseId,
                    FechamentoTurmaId = fechamentoTurmaId,
                    AlunoCodigo = alunoCodigo,
                    CodigoTurma = codigoTurma,
                    Bimestre = bimestre,
                    ConsideraHistorico = consideraHistorico
                });
            return Ok(retorno);
        }

        [HttpPost("recomendacoes")]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(AuditoriaConselhoClasseAlunoDto), 200)]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> SalvarRecomendacoesAlunoFamilia(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto, [FromServices] ISalvarConselhoClasseAlunoRecomendacaoUseCase salvarConselhoClasseAlunoRecomendacaoUseCase)
        {
            return Ok(await salvarConselhoClasseAlunoRecomendacaoUseCase.Executar(conselhoClasseAlunoDto));
        }

        [HttpPost("{conselhoClasseId}/notas/alunos/{codigoAluno}/turmas/{codigoTurma}/bimestres/{bimestre}/fechamento-turma/{fechamentoTurmaId}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> PersistirNotas([FromServices] ISalvarConselhoClasseAlunoNotaUseCase useCase,
           [FromBody] ConselhoClasseNotaDto conselhoClasseNotaDto, string codigoAluno, long conselhoClasseId, long fechamentoTurmaId, string codigoTurma, int bimestre)
        {
            var dto = new SalvarConselhoClasseAlunoNotaDto()
            {
                ConselhoClasseNotaDto = conselhoClasseNotaDto,
                CodigoAluno = codigoAluno,
                ConselhoClasseId = conselhoClasseId,
                FechamentoTurmaId = fechamentoTurmaId,
                CodigoTurma = codigoTurma,
                Bimestre = bimestre
            };

            return Ok(await useCase.Executar(dto));
        }

        [HttpGet("detalhamento/{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
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

        [HttpGet("turmas/{turmaCodigo}/alunos/{alunoCodigo}/consideraHistorico/{consideraHistorico}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ConselhoClasseAlunoResumoDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterConselhoClasseTurmaFinal(string turmaCodigo, string alunoCodigo, bool consideraHistorico, [FromServices] IConsultasConselhoClasse consultasConselhoClasse)
        {
            var retorno = (await consultasConselhoClasse.ObterConselhoClasseTurmaFinal(turmaCodigo, alunoCodigo, consideraHistorico));

            if (retorno.EhNulo())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/turmas/{codigoTurma}/parecer/consideraHistorico/{consideraHistorico}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ParecerConclusivoDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterParecerConclusivoAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, bool consideraHistorico, [FromServices] IObterParecerConclusivoUseCase obterParecerConclusivoUseCase)
            => Ok(await obterParecerConclusivoUseCase.Executar(new ConselhoClasseParecerConclusivoConsultaDto(conselhoClasseId, fechamentoTurmaId, alunoCodigo, codigoTurma, consideraHistorico)));

        [HttpGet("turmas/{codigoTurma}/alunos/{alunoCodigo}/parecer")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ParecerConclusivoDto), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterParecerConclusivoPorTurmaAluno(string codigoTurma, string alunoCodigo, [FromServices] IObterParecerConclusivoAlunoTurmaUseCase obterParecerConclusivoAlunoTurmaUseCase)
            => Ok(await obterParecerConclusivoAlunoTurmaUseCase.Executar(codigoTurma, alunoCodigo));

        [HttpPost("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/parecer")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ParecerConclusivoDto), 200)]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> GerarParecerConclusivoAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, [FromServices] IGerarParecerConclusivoUseCase gerarParecerConclusivoUseCase)
            => Ok(await gerarParecerConclusivoUseCase.Executar(new ConselhoClasseFechamentoAlunoDto() { ConselhoClasseId = conselhoClasseId, FechamentoTurmaId = fechamentoTurmaId, AlunoCodigo = alunoCodigo}));

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/turmas/{codigoTurma}/bimestres/{bimestre}/sintese")]
        [ProducesResponseType(typeof(IEnumerable<ConselhoDeClasseGrupoMatrizDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSintesesConselhoDeClasse(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre, [FromServices] IObterSinteseConselhoDeClasseUseCase obterSintesesConselhoDeClasseUseCase)
        {
            return Ok(await obterSintesesConselhoDeClasseUseCase.Executar(new ConselhoClasseSinteseDto(conselhoClasseId,fechamentoTurmaId,alunoCodigo,codigoTurma,bimestre)));
        }

        [HttpGet("{conselhoClasseId}/fechamentos/{fechamentoTurmaId}/alunos/{alunoCodigo}/turmas/{codigoTurma}/bimestres/{bimestre}/notas")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<ConselhoClasseAlunoNotasConceitosDto>), 200)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterNotasAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre,
            [FromServices] IObterNotasFrequenciaUseCase obterNotasFrequenciaUseCase,
            [FromQuery] bool consideraHistorico = false)
            => Ok(await obterNotasFrequenciaUseCase.Executar(new ConselhoClasseNotasFrequenciaDto(conselhoClasseId, fechamentoTurmaId, alunoCodigo, codigoTurma, bimestre, consideraHistorico)));

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
        public async Task<IActionResult> ImprimirConselhoAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, [FromServices] IImpressaoConselhoClasseAlunoUseCase impressaoConselhoClasseAlunoUseCase, [FromQuery] string frequenciaGlobal = "")
          => Ok(await impressaoConselhoClasseAlunoUseCase.Executar(new FiltroRelatorioConselhoClasseAlunoDto() { ConselhoClasseId = conselhoClasseId, CodigoAluno = alunoCodigo, FechamentoTurmaId = fechamentoTurmaId, FrequenciaGlobal = frequenciaGlobal }));

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

        [HttpGet("ReprocessarSituacaoConselhoClasseAluno")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(IEnumerable<BimestreComConselhoClasseTurmaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ReprocessarSituacaoConselhoClasseAluno(int dreId, [FromServices] IConsolidarConselhoClasseUseCase consolidarConselhoCasseUseCase)
        {
            await consolidarConselhoCasseUseCase.Executar(dreId);
            return Ok();
        }

        [HttpPost("turmas/anos-letivos/{AnoLetivo}/parecer/reprocessar")]
        [Permissao(Permissao.CC_I, Policy = "Bearer")]
        public async Task<IActionResult> ReprocessarParecerConclusivo(int anoLetivo, [FromServices] IReprocessarParecerConclusivoPorAnoUseCase useCase)
        {
            await useCase.Executar(anoLetivo);
            return Ok();
        }

        [HttpGet("ObterTotalAulas/aluno/{codigoAluno}/turma/{codigoTurma}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(IEnumerable<TotalAulasPorAlunoTurmaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTotalAulasPorAlunoTurma(string codigoAluno, string codigoTurma, [FromServices] IObterTotalAulasPorAlunoTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoAluno, codigoTurma));
        }

        [HttpGet("ObterTotalAulasSemFrequencia/turma/{codigoTurma}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(IEnumerable<TotalAulasPorAlunoTurmaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTotalAulasSemFrequenciaPorTurma(string codigoTurma, [FromServices] IObterTotalAulasSemFrequenciaPorTurmaUseCase useCase)
        {
            return Ok(await useCase.Executar("1106", codigoTurma));
        }

        [HttpGet("TotalAulasNaoLancamNota/turma/{codigoTurma}/bimestre/{bimestre}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(IEnumerable<TotalAulasNaoLancamNotaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTotalAulasNaoLancamNotasPorTurmaBimestre(string codigoTurma, int bimestre, string codigoAluno, [FromServices] IObterTotalAulasNaoLancamNotaUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoTurma, bimestre, codigoAluno));
        }

        [HttpGet("TotalCompensacoesComponentesNaoLancamNota/turma/{codigoTurma}/bimestre/{bimestre}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTotalCompensacoesComponentesNaoLancamNota(string codigoTurma, int bimestre, [FromServices] IObterTotalCompensacoesComponenteNaoLancaNotaUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoTurma, bimestre));
        }

        [HttpGet("obter-recomendacoes")]
        [ProducesResponseType(typeof(IEnumerable<RecomendacoesAlunoFamiliaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(204)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRecomendacoes([FromServices] IObterRecomendacoesAlunoFamiliaUseCase useCase)
        {
            var recomendacoes = await useCase.Executar();
            if (recomendacoes.NaoEhNulo())
                return Ok(recomendacoes);
            else
                return StatusCode(204);
        }

        [HttpGet("validar-inconsistencias/turma/{turmaId}/bimestre/{bimestre}")]
        [ProducesResponseType(typeof(IEnumerable<InconsistenciasAlunoFamiliaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ObterAlunosSemNotasRecomendacoes(long turmaId,int bimestre,[FromServices] IObterAlunosSemNotasRecomendacoesUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(turmaId,bimestre)));
        }

        [HttpGet("turma/{turmaId}/pareceres-conclusivos")]
        [ProducesResponseType(typeof(IEnumerable<ParecerConclusivoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ObterPareceresConclusivosTurma(long turmaId, [FromServices] IObterPareceresConclusivosTurmaUseCase useCase, [FromQuery] bool anoLetivoAnterior = false)
        {
            return Ok(await useCase.Executar(turmaId, anoLetivoAnterior));
        }

        [HttpPut("parecer-conclusivo")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(ParecerConclusivoDto), 200)]
        [Permissao(Permissao.CC_A, Policy = "Bearer")]
        public async Task<IActionResult> AlterarParecerConclusivo(AlterarParecerConclusivoDto alterarParecerConclusivo, [FromServices] IAlterarParecerConclusivoUseCase useCase)
        {
            return Ok(await useCase.Executar(alterarParecerConclusivo));
        }

        [HttpGet("turmas/{codigoTurmaRegular}/alunos/{codigoAluno}/relatorios-pap")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestoesDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.RPAP_C, Permissao.CC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionarioPAPConselhoClasse(string codigoTurmaRegular, string codigoAluno, int bimestre, [FromServices] IObterRelatorioPAPConselhoClasseUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoTurmaRegular, codigoAluno, bimestre));
        }

        [HttpGet("anos-letivos/{AnoLetivo}/modalidades/{modalidade}/pareceres-conclusivos")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(IEnumerable<ParecerConclusivoDto>), 200)]
        [Permissao(Permissao.RPC_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPareceresConclusivosAnoLetivoModalidade(int anoLetivo, Modalidade modalidade, [FromServices] IObterPareceresConclusivosAnoLetivoModalidadeUseCase obterPareceresConclusivosUseCase)
         => Ok(await obterPareceresConclusivosUseCase.Executar(anoLetivo, modalidade));
    }
}