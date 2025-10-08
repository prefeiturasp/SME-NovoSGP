using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/busca-ativa")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class BuscaAtivaController : ControllerBase
    {
        [HttpPost("registros-acao")]
        [ProducesResponseType(typeof(IEnumerable<ResultadoRegistroAcaoBuscaAtivaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CCEA_NAAPA_C, Permissao.RABA_NAAPA_I, Policy = "Bearer")]
        public async Task<IActionResult> RegistrarRegistroAcao([FromBody] RegistroAcaoBuscaAtivaDto registroAcaoDto, [FromServices] IRegistrarRegistroAcaoUseCase registrarRegistroAcaoUseCase)
        {
            return Ok(await registrarRegistroAcaoUseCase.Executar(registroAcaoDto));
        }

        [HttpGet("registros-acao/secoes")]
        [ProducesResponseType(typeof(IEnumerable<SecaoQuestionarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CCEA_NAAPA_C, Permissao.RABA_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterSecoesDeRegistroAcao([FromQuery] FiltroSecoesDeRegistroAcao filtro,
            [FromServices] IObterSecoesRegistroAcaoSecaoUseCase obterSecoesRegistroAcaoSecaoUseCase)
        {
            return Ok(await obterSecoesRegistroAcaoSecaoUseCase.Executar(filtro));
        }

        [HttpGet("registros-acao/questionario/{questionarioId}")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CCEA_NAAPA_C, Permissao.RABA_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuestionario(long questionarioId, [FromQuery] long? registroAcaoId, [FromServices] IObterQuestionarioRegistroAcaoUseCase useCase)
        {
            return Ok(await useCase.Executar(questionarioId, registroAcaoId));
        }

        [HttpDelete("registros-acao/{registroAcaoId}")]
        [ProducesResponseType(typeof(RegistroAcaoBuscaAtivaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CCEA_NAAPA_C, Permissao.RABA_NAAPA_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirRegistroAcao(long registroAcaoId, [FromServices] IExcluirRegistroAcaoUseCase useCase)
        {
            return Ok(await useCase.Executar(registroAcaoId));
        }

        [HttpGet("criancas-estudantes/ausentes/registros-acao")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CCEA_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRegistrosAcaoCriancaEstudanteAusente([FromQuery] FiltroRegistrosAcaoCriancasEstudantesAusentesDto filtro,
            [FromServices] IObterRegistrosAcaoCriancaEstudanteAusenteUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("registros-acao")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RABA_NAAPA_C, Permissao.PDA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRegistrosAcao([FromQuery] FiltroRegistrosAcaoDto filtro,
            [FromServices] IObterRegistrosAcaoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("registros-acao/{registroAcaoId}")]
        [ProducesResponseType(typeof(RegistroAcaoBuscaAtivaRespostaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CCEA_NAAPA_C, Permissao.RABA_NAAPA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRegistroAcao(long registroAcaoId, [FromServices] IObterRegistroAcaoPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(registroAcaoId));
        }

        [HttpPut("criancas-estudantes/responsaveis")]
        [ProducesResponseType(typeof(RegistroAcaoBuscaAtivaRespostaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CCEA_NAAPA_A, Policy = "Bearer")]
        public async Task<ObjectResult> AtualizarDadosResponsaveis([FromBody] AtualizarDadosResponsavelDto usuarioDto, [FromServices] IAtualizarDadosResponsaveisUseCase atualizarDadosResponsaveisUseCase)
        {
            return Ok(await atualizarDadosResponsaveisUseCase.Executar(usuarioDto));
        }

        [HttpGet("motivos-ausencia")]
        [ProducesResponseType(typeof(IEnumerable<OpcaoRespostaSimplesDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RBA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterMotivosAusencia([FromServices] IObterOpcoesRespostaMotivoAusenciaBuscaAtivaUseCase useCase) => Ok(await useCase.Executar());
    }
}