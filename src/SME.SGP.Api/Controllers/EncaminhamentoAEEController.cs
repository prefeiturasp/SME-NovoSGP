using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlunoDto = SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar.AlunoDto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/encaminhamento-aee")]
    public class EncaminhamentoAEEController : ControllerBase
    {

        [HttpPost("salvar")]
        [ProducesResponseType(typeof(IEnumerable<ResultadoEncaminhamentoAEEDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> RegistrarEncaminhamento([FromQuery] EncaminhamentoAEEDto encaminhamentoAlunoDto, [FromServices] IRegistrarEncaminhamentoAEEUseCase registrarEncaminhamentoAEEUseCase)
        {
            return Ok(await registrarEncaminhamentoAEEUseCase.Executar(encaminhamentoAlunoDto));
        }

        [HttpGet("secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSecoesPorEtapaDeEncaminhamentoAEE([FromQuery] long etapa, [FromServices] IObterSecoesPorEtapaDeEncaminhamentoAEEUseCase obterSecoesPorEtapaDeEncaminhamentoAEEUseCase)
        {
            return Ok(await obterSecoesPorEtapaDeEncaminhamentoAEEUseCase.Executar(etapa));
        }

        [HttpGet("questionario")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoAeeDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterQuestionario([FromQuery] long questionarioId, [FromQuery] long? encaminhamentoId, [FromServices] IObterQuestionarioEncaminhamentoAeeUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, encaminhamentoId));
        }

        [HttpGet]
        [Route("situacoes")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
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
        //[Permissao(Permissao.DPU_E, Policy = "Bearer")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ExcluirArquivo([FromQuery] long arquivoId, [FromServices] IExcluirArquivoAeeUseCase useCase)
        {
            return Ok(await useCase.Executar(arquivoId));
        }
        
        [HttpGet("instrucoes-modal")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterInstrucoesModal([FromServices] IObterInstrucoesModalUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromServices] IUploadDeArquivoUseCase useCase)
        {
            try
            {
                if (file.Length > 0)
                    return Ok(await useCase.Executar(file, Dominio.TipoArquivo.EncaminhamentoAEE));

                return BadRequest();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterEncaminhamentos([FromQuery] FiltroPesquisaEncaminhamentosAEEDto filtro, [FromServices] IObterEncaminhamentosAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpDelete("{encaminhamentoAeeId}")]
        [ProducesResponseType(typeof(EncaminhamentoAEEDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ExcluirEncaminhamento(long encaminhamentoAeeId, [FromServices] IExcluirEncaminhamentoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoAeeId));
        }
    }
}