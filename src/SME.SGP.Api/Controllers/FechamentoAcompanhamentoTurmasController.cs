using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api
{
    [ApiController]
    [Route("api/v1/fechamentos/acompanhamentos/turmas")]
    [Authorize("Bearer")]
    public class FechamentoAcompanhamentoTurmasController : ControllerBase
    {
        [HttpGet("")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaTurmas([FromQuery] FiltroAcompanhamentoFechamentoTurmasDto filtro, [FromServices] IObterTurmasFechamentoAcompanhamentoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("{turmaId}/fechamentos/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(IEnumerable<StatusTotalFechamentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaTotalStatusFechamentos(long turmaId, int bimestre, [FromServices] IObterFechamentoConsolidadoPorTurmaBimestreUseCase useCase)
        {
            var listaStatus = await useCase.Executar(new FiltroFechamentoConsolidadoTurmaBimestreDto(turmaId, bimestre));

            return Ok(listaStatus);
        }

        [HttpGet("{turmaId}/conselho-classe/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(IEnumerable<StatusTotalFechamentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaTotalStatusConselhosClasse(long turmaId, int bimestre, [FromServices] IObterConselhoClasseConsolidadoPorTurmaBimestreUseCase useCase)
        {
            var listaStatus = await useCase.Executar(new FiltroConselhoClasseConsolidadoTurmaBimestreDto(turmaId, bimestre));

            return Ok(listaStatus);
        }

        [HttpGet("{turmaId}/conselho-classe/bimestres/{bimestre}/alunos")]
        [ProducesResponseType(typeof(IEnumerable<ConselhoClasseAlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public IActionResult ListaPaginadaAlunosPorTurma(long turmaId, int bimestre)
        {
            return Ok(new List<ConselhoClasseAlunoDto>() {
                new ConselhoClasseAlunoDto() { 
                    NumeroChamada = 1,
                    NomeAluno = "Aluno Teste 1",
                    AlunoCodigo = "0000001",
                    SituacaoConselhoClasse = Dominio.SituacaoConselhoClasse.NaoIniciado,
                    FrequenciaGlobal = 100,
                    PodeExpandir = false
                },
                new ConselhoClasseAlunoDto() {
                    NumeroChamada = 2,
                    NomeAluno = "Aluno Teste 2",
                    AlunoCodigo = "0000002",
                    SituacaoConselhoClasse = Dominio.SituacaoConselhoClasse.EmAndamento,
                    FrequenciaGlobal = 100,
                    PodeExpandir = true
                },
                 new ConselhoClasseAlunoDto() {
                    NumeroChamada = 3,
                    NomeAluno = "Aluno Teste 3",
                    AlunoCodigo = "0000003",
                    SituacaoConselhoClasse = Dominio.SituacaoConselhoClasse.Concluido,
                    FrequenciaGlobal = 100,
                    PodeExpandir = true
                },
            });
        }

        [HttpGet("{turmaId}/conselho-classe/bimestres/{bimestre}/alunos/{alunoCodigo}/componentes-curriculares/detalhamento")]
        [ProducesResponseType(typeof(IEnumerable<DetalhamentoComponentesCurricularesAlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public IActionResult DetalhamentoComponentesCurricularesAluno(long turmaId, int bimestre, string alunoCodigo)
        {
            return Ok(new List<DetalhamentoComponentesCurricularesAlunoDto>() {
                new DetalhamentoComponentesCurricularesAlunoDto() {
                    NomeComponenteCurricular = "Componente 1",
                    NotaFechamento = 1,
                    NotaPosConselho = 1,
                    QuantidadeAusencia = 1,
                    QuantidadeCompensacoes = 1,
                    PercentualFrequencia = 1
                },
                new DetalhamentoComponentesCurricularesAlunoDto() {
                    NomeComponenteCurricular = "Componente 2",
                    NotaFechamento = 1,
                    NotaPosConselho = 1,
                    QuantidadeAusencia = 1,
                    QuantidadeCompensacoes = 1,
                    PercentualFrequencia = 1
                },
                new DetalhamentoComponentesCurricularesAlunoDto() {
                    NomeComponenteCurricular = "Componente 3",
                    NotaFechamento = 1,
                    NotaPosConselho = 1,
                    QuantidadeAusencia = 1,
                    QuantidadeCompensacoes = 1,
                    PercentualFrequencia = 1
                },
            });
        }
    }
}
