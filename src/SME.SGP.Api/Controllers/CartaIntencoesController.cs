using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/carta-intencoes")]
    [Authorize("Bearer")]
    public class CartaIntencoesController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CI_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromServices] ICartaIntencoesPersistenciaUseCase useCase, [FromBody] CartaIntencoesPersistenciaDto dto)
        {
            return Ok(await useCase.Executar(dto));
        }

        [HttpGet("turmas/{turmaCodigo}/componente-curricular/{componenteCurricularId}")]
        [ProducesResponseType(typeof(IEnumerable<CartaIntencoesRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CI_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter([FromServices] IObterCartasDeIntencoesPorTurmaEComponenteUseCase useCase, string turmaCodigo, long componenteCurricularId)
        {
            return Ok(await useCase.Executar(new ObterCartaIntencoesDto(turmaCodigo, componenteCurricularId)));
        }

        //[HttpGet("{cartaIntencoesId}/observacoes")]
        //[ProducesResponseType(typeof(IEnumerable<ListarObservacaoCartaIntencoesDto>), 200)]
        //[ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.CI_C, Policy = "Bearer")]
        //public async Task<IActionResult> ListarObservacoes(long cartaIntencoesId, [FromServices] IListarObservacaoCartaIntencoesUseCase listarObservacaoCartaIntencoesUseCase)
        //{
        //    return Ok(await listarObservacaoCartaIntencoesUseCase.Executar(cartaIntencoesId));
        //}

        [HttpGet("turmas/{turmaId}/componente-curricular/{componenteCurricularId}/observacoes")]
        [ProducesResponseType(typeof(IEnumerable<CartaIntencoesObservacaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CI_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarObservacoes([FromServices] IListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase useCase, long turmaId, long componenteCurricularId)
        {
            return Ok(await useCase.Executar(new BuscaCartaIntencoesObservacaoDto(turmaId, componenteCurricularId)));
        }


        [HttpPost("{cartaIntencoesId}/observacoes")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CI_C, Policy = "Bearer")]
        public async Task<IActionResult> SalvarObservacao(long cartaIntencoesId, [FromBody] SalvarObservacaoCartaIntencoesDto dto, [FromServices] ISalvarObservacaoCartaIntencoesUseCase useCase)
        {
            return Ok(await useCase.Executar(cartaIntencoesId, dto));
        }
    }
}
