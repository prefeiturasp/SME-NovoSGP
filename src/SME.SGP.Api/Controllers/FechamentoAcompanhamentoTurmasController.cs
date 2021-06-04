using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
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
        public async Task<IActionResult> ListaTotalStatusFechamentos(long turmaId, int bimestre, int situacaoFechamento, [FromServices] IObterFechamentoConsolidadoPorTurmaBimestreUseCase useCase)
        {
            var listaStatus = await useCase.Executar(new FiltroFechamentoConsolidadoTurmaBimestreDto(turmaId, bimestre, situacaoFechamento));

            return Ok(listaStatus);
        }

        [HttpGet("{turmaId}/conselho-classe/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(IEnumerable<StatusTotalFechamentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaTotalStatusConselhosClasse(long turmaId, int bimestre, int situacaoConselhoClasse, [FromServices] IObterConselhoClasseConsolidadoPorTurmaBimestreUseCase useCase)
        {
            var listaStatus = await useCase.Executar(new FiltroConselhoClasseConsolidadoTurmaBimestreDto(turmaId, bimestre, situacaoConselhoClasse));

            return Ok(listaStatus);
        }

        [HttpGet("{turmaId}/conselho-classe/bimestres/{bimestre}/alunos")]
        [ProducesResponseType(typeof(IEnumerable<ConselhoClasseAlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaAlunosPorTurma(long turmaId, int bimestre, int situacaoConselhoClasse, [FromServices] IObterFechamentoConselhoClasseAlunosPorTurmaUseCase useCase)
        {
            var conselhoClasseAlunos = await useCase.Executar(new FiltroConselhoClasseConsolidadoTurmaBimestreDto(turmaId, bimestre, situacaoConselhoClasse));

            return Ok(conselhoClasseAlunos);
        }

        [HttpGet("{turmaId}/conselho-classe/bimestres/{bimestre}/alunos/{alunoCodigo}/componentes-curriculares/detalhamento")]
        [ProducesResponseType(typeof(IEnumerable<DetalhamentoComponentesCurricularesAlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public async Task<IActionResult> DetalhamentoComponentesCurricularesAluno(long turmaId, int bimestre, string alunoCodigo, [FromServices] IObterDetalhamentoFechamentoConselhoClasseAlunoUseCase useCase)
        {
            var listaStatus = await useCase.Executar(new FiltroConselhoClasseConsolidadoDto(turmaId, bimestre, alunoCodigo));

            return Ok(listaStatus);
        }

        [HttpGet("{turmaId}/fechamento/bimestres/{bimestre}/componentes-curriculares")]
        [ProducesResponseType(typeof(IEnumerable<ConsolidacaoTurmaComponenteCurricularDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterComponentesFechamentoConsolidadoPorTurmaBimestre(long turmaId, int bimestre, int situacaoConselhoClasse, [FromServices] IObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase useCase)
        {
            var listaComponentes = await useCase.Executar(new FiltroComponentesFechamentoConsolidadoDto(turmaId, bimestre, situacaoConselhoClasse));

            return Ok(listaComponentes);
        }

        [HttpGet("{turmaId}/fechamento/bimestres/{bimestre}/componentes-curriculares/{componenteCurricularId}/pendencias")]
        [ProducesResponseType(typeof(IEnumerable<PendenciaParaFechamentoConsolidadoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPendenciasParaFechamentoConsolidado(long turmaId, int bimestre, long componenteCurricularId, [FromServices] IObterPendenciasParaFechamentoConsolidadoUseCase useCase)
        {
            var listaPendencias = await useCase.Executar(new FiltroPendenciasFechamentoConsolidadoDto(turmaId, bimestre, componenteCurricularId));

            return Ok(listaPendencias);
        }

        [HttpGet("pendencias/{pendenciaId}/detalhamentos")]
        [ProducesResponseType(typeof(DetalhamentoPendenciaFechamentoRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDetalhamentoPendenciasFechamento(long pendenciaId, [FromServices] IObterDetalhamentoPendenciaFechamentoConsolidadoUseCase useCase)
        {
            return Ok(await useCase.Executar(pendenciaId));
        }

        [HttpGet("pendencias/{pendenciaId}/aulas/detalhamentos")]
        [ProducesResponseType(typeof(DetalhamentoPendenciaAulaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDetalhamentoPendenciasAula(long pendenciaId, [FromServices] IObterDetalhamentoPendenciaAulaUseCase useCase)
        {
            return Ok(await useCase.Executar(pendenciaId));
        }
    }
}
