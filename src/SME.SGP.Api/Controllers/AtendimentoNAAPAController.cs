using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atendimento-naapa")]
    [Authorize("Bearer")]
    public class AtendimentoNAAPAController : ControllerBase
    {
        [HttpPost("salvar")]
        [ProducesResponseType(typeof(IEnumerable<ResultadoAtendimentoNAAPADto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_I, Policy = "Bearer")]
        public async Task<IActionResult> RegistrarEncaminhamento([FromBody] AtendimentoNAAPADto atendimentoNAAPADto, [FromServices] IRegistrarAtendimentoNAAPAUseCase registrarAtendimentoNAAPAUseCase)
        {
            return Ok(await registrarAtendimentoNAAPAUseCase.Executar(atendimentoNAAPADto));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEncaminhamentosNAAPA([FromQuery] FiltroAtendimentoNAAPADto filtro,
            [FromServices] IObterAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesDeEncaminhamentoNAAPA([FromQuery] FiltroSecoesAtendimentoNAAPA filtro,
            [FromServices] IObterSecoesAtendimentoSecaoNAAPAUseCase obterSecoesDeAtendimentoNAAPAUseCase)
        {
            return Ok(await obterSecoesDeAtendimentoNAAPAUseCase.Executar(filtro));
        }

        [HttpGet("{encaminhamentoNAAPAId}/secoes-itinerancia")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesItineranciaDeEncaminhamentoNAAPA(long encaminhamentoNAAPAId,
            [FromServices] IObterSecoesItineranciaDeAtendimentoNAAPAUseCase obterSecoesItineranciaDeAtendimentoNAAPAUseCase)
        {
            return Ok(await obterSecoesItineranciaDeAtendimentoNAAPAUseCase.Executar(encaminhamentoNAAPAId));
        }

        [HttpGet("questionario")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionario([FromQuery] long questionarioId, [FromQuery] long? encaminhamentoId, [FromQuery] string codigoAluno, [FromQuery] string codigoTurma, [FromServices] IObterQuestionarioAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, encaminhamentoId, codigoAluno, codigoTurma));
        }

        [HttpGet("situacoes")]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public IActionResult ObterSituacoes()
        {
            var lista = EnumExtensao.ListarDto<SituacaoNAAPA>().OrderBy(situacao => situacao.Descricao);

            return Ok(lista);
        }

        [HttpGet("prioridades")]
        [ProducesResponseType(typeof(IEnumerable<PrioridadeAtendimentoNAAPADto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPrioridades([FromServices] IObterPrioridadeAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpDelete("arquivo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirArquivo([FromQuery] Guid arquivoCodigo, [FromServices] IExcluirArquivoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(arquivoCodigo));
        }

        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_I, Policy = "Bearer")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromServices] IUploadDeArquivoUseCase useCase)
        {
            if (file.Length > 0)
                return Ok(await useCase.Executar(file, Dominio.TipoArquivo.EncaminhamentoNAAPA));

            return BadRequest();
        }

        [HttpDelete("{encaminhamentoNAAPAId}")]
        [ProducesResponseType(typeof(AtendimentoNAAPADto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirEncaminhamento(long encaminhamentoNAAPAId, [FromServices] IExcluirAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoNAAPAId));
        }

        [HttpGet("{encaminhamentoId}")]
        [ProducesResponseType(typeof(AtendimentoNAAPARespostaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEncaminhamento(long encaminhamentoId, [FromServices] IObterAtendimentoNAAPAPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoId));
        }

        [HttpGet("questionarioItinerario")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionarioItinerario([FromQuery] long questionarioId, [FromQuery] long? encaminhamentoSecaoId, [FromServices] IObterQuestionarioItinerarioAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, encaminhamentoSecaoId));
        }

        [HttpDelete("{encaminhamentoNAAPAId}/secoes-itinerancia/{secaoItineranciaId}")]
        [ProducesResponseType(typeof(AtendimentoNAAPADto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirSecaoItinerancia(long encaminhamentoNAAPAId, long secaoItineranciaId, [FromServices] IExcluirSecaoItineranciaAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoNAAPAId, secaoItineranciaId));
        }

        [HttpPost("salvarItinerario")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_I, Policy = "Bearer")]
        public async Task<IActionResult> RegistrarEncaminhamentoItinerario([FromBody] AtendimentoNAAPAItineranciaDto atendimentoNAAPAItineranciaDto, [FromServices] IRegistrarAtendimentoItinerarioNAAPAUseCase registrarAtendimentoItinerarioNAAPAUseCase)
        {
            return Ok(await registrarAtendimentoItinerarioNAAPAUseCase.Executar(atendimentoNAAPAItineranciaDto));
        }

        [HttpGet("{encaminhamentoNAAPAId}/situacao")]
        [ProducesResponseType(typeof(SituacaoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSituacao(long encaminhamentoNAAPAId, [FromServices] IObterSituacaoAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoNAAPAId));
        }

        [HttpPost("encerrar")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_A, Policy = "Bearer")]
        public async Task<IActionResult> EncerrarEncaminhamento([FromBody] EncerramentoAtendimentoDto parametros, [FromServices] IEncerrarAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(parametros.EncaminhamentoId, parametros.MotivoEncerramento));
        }

        [HttpPost("reabrir/{encaminhamentoNAAPAId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_A, Policy = "Bearer")]
        public async Task<IActionResult> ReabrirEncaminhamento(long encaminhamentoNAAPAId, [FromServices] IReabrirAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoNAAPAId));
        }

        [HttpGet("fluxos-alerta")]
        [ProducesResponseType(typeof(IEnumerable<OpcaoRespostaSimplesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFluxosAlerta([FromServices] IObterOpcoesRespostaFluxoAlertaAtendimentosNAAPAUseCase useCase) => Ok(await useCase.Executar());

        [HttpGet("portas-entrada")]
        [ProducesResponseType(typeof(IEnumerable<OpcaoRespostaSimplesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPortasEntrada([FromServices] IObterOpcoesRespostaPortaEntradaAtendimentosNAAPAUseCase useCase) => Ok(await useCase.Executar());

        [HttpPost("imprimir-detalhado")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ImprimirDetalhado([FromBody] FiltroRelatorioAtendimentoNaapaDetalhadoDto filtro, [FromServices] IRelatorioAtendimentoNaapaDetalhadoUseCase detalhadoUseCase)
        {
            return Ok(await detalhadoUseCase.Executar(filtro));
        }

        [HttpGet("{encaminhamentoNAAPAId}/observacoes")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterObservacoes(long encaminhamentoNAAPAId,
            [FromServices] IObterObservacoesDeAtendimentoNAAPAUseCase obterObservacoesDeAtendimentoNAAPAUseCase)
        {
            return Ok(await obterObservacoesDeAtendimentoNAAPAUseCase.Executar(encaminhamentoNAAPAId));
        }

        [HttpPost("salvar-observacao")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> SalvarObservacao([FromBody]AtendimentoNAAPAObservacaoSalvarDto filtro,[FromServices]ISalvarObservacoesDeAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpDelete("excluir-observacao/{observacaoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ExcluirObservacao(long observacaoId, [FromServices] IExcluirObservacoesDeAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(observacaoId));
        }

        [HttpGet("{encaminhamentoNAAPAId}/historico-alteracoes")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterHistoricoDeAlteracoes(long encaminhamentoNAAPAId,
            [FromServices] IObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoNAAPAId));
        }

        [HttpGet("aluno/{codigoAluno}/existe-encaminhamento-ativo")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ExisteEncaminhamentoAtivoParaAluno(string codigoAluno, [FromServices] IExisteAtendimentoNAAPAAtivoParaAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoAluno));
        }

        [HttpDelete("secoes-itinerancia/arquivo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirArquivoItinerancia([FromQuery] Guid arquivoCodigo, [FromServices] IExcluirArquivoItineranciaNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(arquivoCodigo));
        }

        [HttpPost("secoes-itinerancia/upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_I, Policy = "Bearer")]
        public async Task<IActionResult> UploadItinerancia([FromForm] IFormFile file, [FromServices] IUploadDeArquivoUseCase useCase)
        {
            if (file.Length > 0)
                return Ok(await useCase.Executar(file, Dominio.TipoArquivo.ItineranciaEncaminhamentoNAAPA));

            return BadRequest();
        }

        [HttpGet("aluno/{codigoAluno}/registros-acao")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<RegistroAcaoBuscaAtivaNAAPADto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRegistrosDeAcaoParaAluno(string codigoAluno, [FromServices] IObterRegistrosDeAcaoParaNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoAluno));
        }

        [HttpGet("{encaminhamentoId}/anexos/tipos-impressao")]
        [ProducesResponseType(typeof(IEnumerable<ImprimirAnexoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTiposDeImprimirAnexos(long encaminhamentoId, [FromServices] IObterTiposDeImprimirAnexosNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoId));
        }

        [HttpGet("secoes-itinerancia/profissionais-envolvidos")]
        [ProducesResponseType(typeof(IEnumerable<FuncionarioUnidadeDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterProfissionaisEnvolvidosAtendimento([FromQuery] FiltroBuscarProfissionaisEnvolvidosAtendimentoNAAPA filtro,
            [FromServices] IObterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase obterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase)
        {
            return Ok(await obterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase.Executar(filtro));
        }
    }
}