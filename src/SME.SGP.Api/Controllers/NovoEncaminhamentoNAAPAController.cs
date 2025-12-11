using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/novo-encaminhamento-naapa")]
    [Authorize("Bearer")]
    public class NovoEncaminhamentoNAAPAController : ControllerBase
    {
        [HttpGet("secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ENC_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesDeEncaminhamento([FromQuery] long? encaminhamentoNaapaId, long? tipoQuestionario,
           [FromServices] IObterSecoesAtendimentoIndividualNAAPAUseCase obterSecoesDeEncaminhamentoNAAPAUseCase)
        {
            return Ok(await obterSecoesDeEncaminhamentoNAAPAUseCase.Executar(encaminhamentoNaapaId, tipoQuestionario));
        }

        [HttpGet("questionario")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ENC_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionario([FromQuery] long questionarioId, [FromQuery] long? encaminhamentoId, [FromQuery] string codigoAluno, [FromQuery] string codigoTurma, [FromServices] IObterQuestionarioAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, encaminhamentoId, codigoAluno, codigoTurma));
        }

        [HttpGet("aluno/{codigoAluno}/existe-encaminhamento-ativo")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ENC_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ExisteEncaminhamentoAtivoParaAluno(string codigoAluno, [FromServices] IExisteAtendimentoNAAPAAtivoParaAlunoUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoAluno));
        }


        [HttpGet("{encaminhamentoId}")]
        [ProducesResponseType(typeof(AtendimentoNAAPARespostaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ENC_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEncaminhamento(long encaminhamentoId, [FromServices] IObterAtendimentoNAAPAPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoId));
        }

        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ENC_NAAPA_I, Policy = "Bearer")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromServices] IUploadDeArquivoUseCase useCase)
        {
            if (file.Length > 0)
                return Ok(await useCase.Executar(file, Dominio.TipoArquivo.AtendimentoNAAPA));

            return BadRequest();
        }

        [HttpGet("obterEncaminhamentoPorTipo")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ENC_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterEncaminhamentosPaginados([FromQuery] FiltroNovoEncaminhamentoNAAPADto filtro,
            [FromServices] IObterNovosEncaminhamentosNAAPAPorTipoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpDelete("arquivo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ENC_NAAPA_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirArquivo([FromQuery] Guid arquivoCodigo, [FromServices] IExcluirArquivoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(arquivoCodigo));
        }

        [HttpPost("salvar")]
        [ProducesResponseType(typeof(IEnumerable<ResultadoNovoEncaminhamentoNAAPADto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.ENC_NAAPA_I, Permissao.ENC_NAAPA_A, Policy = "Bearer")]
        public async Task<IActionResult> RegistrarNovoEncaminhamento([FromBody] NovoEncaminhamentoNAAPADto encaminhamentoNAAPADto, [FromServices] IRegistrarNovoEncaminhamentoNAAPAUseCase registrarNovoEncaminhamentoNAAPAUseCase)
        {
            return Ok(await registrarNovoEncaminhamentoNAAPAUseCase.Executar(encaminhamentoNAAPADto));
        }

        [HttpDelete("{encaminhamentoNAAPAId}")]
        [ProducesResponseType(typeof(AtendimentoNAAPADto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ENC_NAAPA_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirEncaminhamento(long encaminhamentoNAAPAId, [FromServices] IExcluirAtendimentoNAAPAUseCase useCase)
        {
            return Ok(await useCase.Executar(encaminhamentoNAAPAId));
        }

        [HttpGet("situacoes")]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NAAPA_C, Policy = "Bearer")]
        public IActionResult ObterSituacoes()
        {
            var lista = EnumExtensao.ListarDto<SituacaoNovoEncaminhamentoNAAPA>().OrderBy(situacao => situacao.Descricao);

            return Ok(lista);
        }
    }
}