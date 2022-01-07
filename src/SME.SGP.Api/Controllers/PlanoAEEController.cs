using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
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
    [Route("api/v1/plano-aee")]
    public class PlanoAEEController : ControllerBase
    {

        [HttpGet]
        [Route("situacoes")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public IActionResult ObterSituacoes()
        {
            var situacoes = Enum.GetValues(typeof(SituacaoPlanoAEE))
                        .Cast<SituacaoPlanoAEE>()
                        .Select(d => new { codigo = (int)d, descricao = d.Name() })
                        .ToList();
            return Ok(situacoes);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<PlanoAEEResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPlanosAEE([FromQuery] FiltroPlanosAEEDto filtro, [FromServices] IObterPlanosAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet]
        [Route("estudante/{codigoEstudante}")]
        [ProducesResponseType(typeof(PlanoAEEDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPlanoAeeEstudante(string codigoEstudante, [FromServices] IObterPlanoAEEPorCodigoEstudanteUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoEstudante));
        }

        [HttpGet]
        [Route("{planoAeeId}")]
        [ProducesResponseType(typeof(PlanoAEEDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPlanoAee(long? planoAeeId, [FromQuery] string turmaCodigo, [FromQuery] bool? historico, [FromServices] IObterPlanoAEEPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroPesquisaQuestoesPorPlanoAEEIdDto(planoAeeId, turmaCodigo, historico ?? false)));
        }

        [HttpGet]
        [Route("versao/{versaoPlanoId}")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPlanoAeePorVersao(long versaoPlanoId, [FromQuery] string turmaCodigo, [FromQuery] long questionarioId, [FromServices] IObterQuestoesPlanoAEEPorVersaoUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroPesquisaQuestoesPlanoAEEDto(questionarioId, versaoPlanoId, turmaCodigo)));
        }

        [HttpPost("salvar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_A, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] PlanoAEEPersistenciaDto planoAeeDto, [FromServices] ISalvarPlanoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAeeDto));
        }

        [HttpGet("estudante/{codigoEstudante}/existe")]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public async Task<IActionResult> VerificarExistenciaPlanoAEEPorEstudante(string codigoEstudante, [FromServices] IVerificarExistenciaPlanoAEEPorEstudanteUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoEstudante));
        }

        [HttpGet("{planoAEEId}/reestruturacoes")]
        [ProducesResponseType(typeof(IEnumerable<PlanoAEEReestruturacaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterReestruturacoesPlanoAEE(long planoAEEId, [FromServices] IObterRestruturacoesPlanoAEEPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAEEId));
        }

        [HttpGet("{planoAEEId}/versoes/reestruturacao/{reestruturacaoId}")]
        [ProducesResponseType(typeof(IEnumerable<PlanoAEEDescricaoVersaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterVersoes(long planoAEEId, long reestruturacaoId, [FromServices] IObterVersoesPlanoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroVersoesPlanoAEEDto(planoAEEId, reestruturacaoId)));
        }      

        [HttpGet("{planoAeeId}/parecer")]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterParecerPlanoAEE(long planoAEEId, [FromServices] IObterParecerPlanoAEEPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAEEId));
        }

        [HttpPost("encerrar-plano")]
        [ProducesResponseType(typeof(RetornoEncerramentoPlanoAEEDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_A, Policy = "Bearer")]
        public async Task<IActionResult> EncerrarPlanoAEE([FromQuery] long planoAEEId, [FromServices] IEncerrarPlanoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAEEId));
        }

        [HttpPost("{planoAeeId}/parecer/cp")]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_A, Policy = "Bearer")]
        public async Task<IActionResult> CadastrarParecerCPPlanoAEE(long planoAEEId, [FromBody] PlanoAEECadastroParecerDto planoAEEParecer, [FromServices] ICadastrarParecerCPPlanoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAEEId, planoAEEParecer));
        }

        [HttpPost("{planoAeeId}/parecer/paai")]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_A, Policy = "Bearer")]
        public async Task<IActionResult> CadastrarDevolutivaPAAIPlanoAEE(long planoAEEId, [FromBody] PlanoAEECadastroParecerDto planoAEEParecer, [FromServices] ICadastrarParecerPAAIPlanoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAEEId, planoAEEParecer));
        }

        [HttpPost("atribuir-responsavel")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_A, Policy = "Bearer")]
        public async Task<IActionResult> AtribuirResponsavelPlanoAEE([FromBody] AtribuirResponsavelPlanoAEEDto parametros, [FromServices] IAtribuirResponsavelPlanoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(parametros.PlanoAEEId, parametros.ResponsavelRF));
        }

        [HttpPost("devolver")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_A, Policy = "Bearer")]
        public async Task<IActionResult> DevolverPlanoAEE([FromBody] DevolucaoPlanoAEEDto devolucao, [FromServices] IDevolverPlanoAEEUseCase useCase)
        {
            await useCase.Executar(devolucao);

            return Ok(new RetornoBaseDto("Plano devolvido com sucesso"));
        }
    }
}