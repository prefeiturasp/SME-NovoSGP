using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
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
        public async Task<IActionResult> ObterPlanosAEE([FromQuery] FiltroPlanosAEEDto filtro, [FromServices] IObterPlanosAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet]
        [Route("estudante/{codigoEstudante}")]
        [ProducesResponseType(typeof(PlanoAEEDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterPlanoAeeEstudante(string codigoEstudante, [FromServices] IObterPlanoAEEPorCodigoEstudanteUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoEstudante));
        }

        [HttpGet]
        [Route("{planoAeeId}")]
        [ProducesResponseType(typeof(PlanoAEEDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterPlanoAee(long? planoAeeId, [FromServices] IObterPlanoAEEPorIdUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAeeId));
        }

        [HttpGet]
        [Route("versao/{versaoPlanoId}")]
        [ProducesResponseType(typeof(IEnumerable<QuestaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterPlanoAeePorVersao(long versaoPlanoId, [FromServices] IObterQuestoesPlanoAEEPorVersaoUseCase useCase)
        {
            return Ok(await useCase.Executar(versaoPlanoId));
        }

        [HttpPost("salvar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Salvar([FromBody] PlanoAEEPersistenciaDto planoAeeDto, [FromServices] ISalvarPlanoAEEUseCase usecase)
        {
            return Ok(await usecase.Executar(planoAeeDto));
        }

        [HttpGet("estudante/{codigoEstudante}/existe")]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> VerificarExistenciaPlanoAEEPorEstudante(string codigoEstudante, [FromServices] IVerificarExistenciaPlanoAEEPorEstudanteUseCase useCase)
        {
            return Ok(await useCase.Executar(codigoEstudante));
        }

        [HttpGet("{planoAEEId}/reestruturacoes")]
        [ProducesResponseType(typeof(IEnumerable<PlanoAEEReestruturacaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterReestruturacoes(long planoAEEId)
        {
            return Ok(new List<PlanoAEEReestruturacaoDto>()
            {
                new PlanoAEEReestruturacaoDto() { Id = 1, Semestre = 1, Data = new DateTime(2021, 1, 10), DescricaoSimples = "Alteradas as atividades que o aluno fazia", Descricao = "<b>Alteradas as atividades que o aluno fazia</b>", Versao = "v1 - 05/01/2021", VersaoId = 10},
                new PlanoAEEReestruturacaoDto() { Id = 2, Semestre = 1, Data = new DateTime(2021, 1, 31), DescricaoSimples = "Alterado o periodo de vigência do plano", Descricao = "<b>Alterado o periodo de vigência do plano</b>", Versao = "v2 - 30/01/2021", VersaoId = 11},
                new PlanoAEEReestruturacaoDto() { Id = 3, Semestre = 1, Data = new DateTime(2021, 2, 05), DescricaoSimples = "Alterado dias e horarios do aluno no AEE", Descricao = "<b>Alterado dias e horarios do aluno no AEE</b>", Versao = "v3 - 04/01/2021", VersaoId = 12},
                new PlanoAEEReestruturacaoDto() { Id = 4, Semestre = 2, Data = new DateTime(2021, 2, 15), DescricaoSimples = "Alterado encaminhamento do aluno", Descricao = "<b>Alterado encaminhamento do aluno</b>", Versao = "v4 - 13/01/2021", VersaoId = 13},
            });
        }

        [HttpGet("{planoAEEId}/versoes")]
        [ProducesResponseType(typeof(IEnumerable<PlanoAEEDescricaoVersaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterVersoes(long planoAEEId)
        {
            return Ok(new List<PlanoAEEDescricaoVersaoDto>()
            {
                new PlanoAEEDescricaoVersaoDto() { Id = 10, Descricao = "v1 - 05/01/2021" },
                new PlanoAEEDescricaoVersaoDto() { Id = 11, Descricao = "v2 - 30/01/2021" },
                new PlanoAEEDescricaoVersaoDto() { Id = 12, Descricao = "v3 - 04/01/2021" },
                new PlanoAEEDescricaoVersaoDto() { Id = 13, Descricao = "v4 - 13/01/2021" },
            });
        }

        [HttpPost("{planoAEEId}/reestruturacoes")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> SalvarReestruturacao([FromBody] PlanoAEEReestrutucacaoPersistenciaDto planoAeeReestruturacaoDto)
        {
            return Ok();

        [HttpGet]
        [Route("versoes-plano")]
        [ProducesResponseType(typeof(IEnumerable<PlanoAEEVersaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterVersoesPlanoAEE([FromQuery] FiltroVersoesPlanoAEEDto filtro, [FromServices] IObterVersoesPlanoAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }
    }
}