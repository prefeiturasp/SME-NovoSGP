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
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/encaminhamento-aee")]
    [Authorize("Bearer")]
    public class EncaminhamentoAEEController : ControllerBase
    {

        [HttpPost("salvar")]
        [ProducesResponseType(typeof(IEnumerable<ResultadoEncaminhamentoAEEDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_I, Policy = "Bearer")]
        public async Task<IActionResult> RegistrarEncaminhamento([FromBody] EncaminhamentoAeeDto encaminhamentoAEEDto, [FromServices] IRegistrarEncaminhamentoAEEUseCase registrarEncaminhamentoAEEUseCase)
        {
            return Ok(await registrarEncaminhamentoAEEUseCase.Executar(encaminhamentoAEEDto));
        }

        [HttpGet("secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesPorEtapaDeEncaminhamentoAEE([FromQuery] long encaminhamentoAeeId, [FromServices] IObterSecoesPorEtapaDeEncaminhamentoAEEUseCase obterSecoesPorEtapaDeEncaminhamentoAEEUseCase)
        {
            return Ok(await obterSecoesPorEtapaDeEncaminhamentoAEEUseCase.Executar(encaminhamentoAeeId));
        }

        [HttpGet("questionario")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionario([FromQuery] long questionarioId, [FromQuery] long? encaminhamentoId, [FromQuery] string codigoAluno, [FromQuery] string codigoTurma, [FromServices] IObterQuestionarioEncaminhamentoAeeUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, encaminhamentoId, codigoAluno, codigoTurma));
        }

        [HttpGet]
        [Route("situacoes")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public IActionResult ObterSituacoes()
        {
            var situacoes = Enum.GetValues(typeof(SituacaoAEE))
                        .Cast<SituacaoAEE>()
                        .Select(d => new { codigo = (int)d, descricao = d.Name() })
                        .ToList();
            return Ok(situacoes);
        }

        [HttpDelete("arquivo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirArquivo([FromQuery] Guid arquivoCodigo, [FromServices] IExcluirArquivoAeeUseCase useCase)
        {
            return Ok(await useCase.Executar(arquivoCodigo));
        }

        [HttpGet("instrucoes-modal")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterInstrucoesModal([FromServices] IObterInstrucoesModalUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_I, Policy = "Bearer")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromServices] IUploadDeArquivoUseCase useCase)
        {
            if (file.Length > 0)
                return Ok(await useCase.Executar(file, Dominio.TipoArquivo.EncaminhamentoAEE));

            return BadRequest();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEncaminhamentos([FromQuery] FiltroPesquisaEncaminhamentosAEEDto filtro, [FromServices] IObterEncaminhamentosAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("responsaveis")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioEolRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterResponsaveis([FromQuery] FiltroPesquisaEncaminhamentosAEEDto filtro, [FromServices] IObterResponsaveisEncaminhamentosAEE useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpDelete("{encaminhamentoAeeId}")]
        [ProducesResponseType(typeof(EncaminhamentoAeeDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirEncaminhamento(long encaminhamentoAeeId, [FromServices] IExcluirEncaminhamentoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoAeeId));
        }

        [HttpGet("{encaminhamentoId}")]
        [ProducesResponseType(typeof(EncaminhamentoAEERespostaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEncaminhamento(long encaminhamentoId, [FromServices] IObterEncaminhamentoPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoId));
        }

        [HttpPost("encerrar")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_A, Policy = "Bearer")]
        public async Task<IActionResult> EncerrarEncaminhamento([FromBody] EncerramentoEncaminhamentoDto parametros, [FromServices] IEncerrarEncaminhamentoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(parametros.EncaminhamentoId, parametros.MotivoEncerramento));
        }

        [HttpPost("enviar-analise/{encaminhamentoId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_A, Policy = "Bearer")]
        public async Task<IActionResult> EnviarParaAnaliseEncaminhamento(long encaminhamentoId, [FromServices] IEnviarParaAnaliseEncaminhamentoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoId));
        }

        [HttpPost("atribuir-responsavel")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_A, Policy = "Bearer")]
        public async Task<IActionResult> AtribuirResponsavelEncaminhamento([FromBody] AtribuirResponsavelEncaminhamentoDto parametros, [FromServices] IAtribuirResponsavelEncaminhamentoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(parametros.EncaminhamentoId, parametros.RfResponsavel));
        }

        [HttpGet("estudante/pode-cadastrar")]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEncaminhamento([FromQuery] FiltroEncaminhamentoAeeDto filtroEncaminhamentoAee,
            [FromServices] IVerificaPodeCadastrarEncaminhamentoAEEParaEstudanteUseCase useCase)
        {
            return Ok(await useCase.Executar(filtroEncaminhamentoAee));
        }

        [HttpGet]
        [Route("estudante/situacao")]
        [ProducesResponseType(typeof(SituacaoEncaminhamentoPorEstudanteDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSituacaoEncaminhamentoPorEstudante([FromQuery] FiltroEncaminhamentoAeeDto filtroEncaminhamentoAee,
            [FromServices] IObterSituacaoEncaminhamentoPorEstudanteUseCase useCase)
        {
            return Ok(await useCase.Executar(filtroEncaminhamentoAee));
        }

        [HttpPost("remover-responsavel/{encaminhamentoId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_A, Policy = "Bearer")]
        public async Task<IActionResult> AtribuirResponsavelEncaminhamento(long encaminhamentoId, [FromServices] IRemoverResponsavelEncaminhamentoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoId));
        }

        [HttpPost]
        [Route("responsavel/pesquisa")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<UsuarioEolRetornoDto>), 200)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> PesquisaResponsavel([FromBody] FiltroPesquisaFuncionarioDto filtro, [FromServices] IPesquisaResponsavelEncaminhamentoPorDreUEUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }


        [HttpPost]
        [Route("responsavel-plano/pesquisa")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<UsuarioEolRetornoDto>), 200)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> PesquisaResponsavelPlano([FromBody] FiltroPesquisaFuncionarioDto filtro, [FromServices] IPesquisaResponsavelPlanoPorDreUEUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpPost("concluir/{encaminhamentoId}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_A, Policy = "Bearer")]
        public async Task<IActionResult> ConcluirEncaminhamento(long encaminhamentoId, [FromServices] IConcluirEncaminhamentoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoId));
        }

        [HttpPost("devolver")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_A, Policy = "Bearer")]
        public async Task<IActionResult> DevolverEncaminhamento([FromBody] DevolucaoEncaminhamentoAEEDto devolucaoDto, [FromServices] IDevolverEncaminhamentoUseCase useCase)
        {
            await useCase.Executar(devolucaoDto);

            return Ok(new RetornoBaseDto("Encaminhamento devolvido com sucesso"));
        }

        [HttpGet]
        [Route("paai")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioEolRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPAAI([FromQuery] string codigoDre, [FromServices] IObterPAAIPorDreUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoDre));
        }
        
        [HttpPost("imprimir-detalhado")]
        [ProducesResponseType(typeof(bool), 200)]
        [Permissao(Permissao.AEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ImpressaoRelatorioEncaminhamentoAeeDetalhado(long[] Ids, [FromServices] IRelatorioEncaminhamentoAeeDetalhadoUseCase detalhadoUseCase)
        {
            return Ok(await detalhadoUseCase.Executar(new FiltroRelatorioEncaminhamentoAeeDetalhadoDto() { Ids = Ids}));
        }
            
    }
}
