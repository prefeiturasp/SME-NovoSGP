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
        public async Task<IActionResult> ObterPlanoAee(long? planoAeeId, [FromQuery] string turmaCodigo, [FromServices] IObterPlanoAEEPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroPesquisaQuestoesPorPlanoAEEIdDto(planoAeeId, turmaCodigo)));
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
        public async Task<IActionResult> Salvar([FromBody] PlanoAEEPersistenciaDto planoAeeDto, [FromServices] ISalvarPlanoAEEUseCase usecase)
        {
            return Ok(await usecase.Executar(planoAeeDto));
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
        
        [HttpGet("{planoAeeId}/devolutiva")]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterDevolutivaPlanoAEE(long planoAEEId, [FromServices] IObterDevolutivaPlanoAEEPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAEEId));
        }

        [HttpPost("encerrar-plano")]
        [ProducesResponseType(typeof(RetornoEncerramentoPlanoAEEDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_A, Policy = "Bearer")]
        public async Task<IActionResult> EncerrarPlanoAEE([FromQuery] long planoAEEId, [FromServices] IEncerrarPlanoAEEUseCase usecase)
        {
            return Ok(await usecase.Executar(planoAEEId));
        }

        [HttpPost("{planoAeeId}/devolutiva/cp")]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_A, Policy = "Bearer")]
        public async Task<IActionResult> CadastrarDevolutivaCPPlanoAEE(long planoAEEId, [FromBody] PlanoAEECadastroDevolutivaDto planoAEEDevolutivaDto, [FromServices] ICadastrarDevolutivaCPPlanoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAEEId, planoAEEDevolutivaDto));
        }

        [HttpPost("{planoAeeId}/devolutiva/paai")]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_A, Policy = "Bearer")]
        public async Task<IActionResult> CadastrarDevolutivaPAAIPlanoAEE(long planoAEEId, [FromBody] PlanoAEECadastroDevolutivaDto planoAEEDevolutivaDto, [FromServices] ICadastrarDevolutivaPAAIPlanoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAEEId, planoAEEDevolutivaDto));
        }

        [HttpPost("atribuir-responsavel")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_A, Policy = "Bearer")]
        public async Task<IActionResult> AtribuirResponsavelPlanoAEE([FromBody] AtribuirResponsavelPlanoAEEDto parametros, [FromServices] IAtribuirResponsavelPlanoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(parametros.PlanoAEEId, parametros.ResponsavelRF));
        }
    }
}