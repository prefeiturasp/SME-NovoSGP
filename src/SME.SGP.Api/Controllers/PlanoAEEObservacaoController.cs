using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/plano-aee/observacoes")]
    public class PlanoAEEObservacaoController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PlanoAEEObservacaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterObservacoesPlanosAEE([FromQuery] long planoAEEId, [FromServices] IObterPlanoAEEObservacaoUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAEEId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] PersistenciaPlanoAEEObservacaoDto dto, [FromServices] ICriarPlanoAEEObservacaoUseCase useCase)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody] PersistenciaPlanoAEEObservacaoDto dto, [FromServices] IAlterarPlanoAEEObservacaoUseCase useCase)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public async Task<IActionResult> Excluir(long id, [FromServices] IExcluirPlanoAEEObservacaoUseCase useCase)
        {
            return Ok(await useCase.Executar(id));
        }
    }
}
