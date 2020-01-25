using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/fechamentos/reaberturas")]
    [Authorize("Bearer")]
    public class FechamentoReaberturaController : ControllerBase
    {
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(string), 200)]
        [Permissao(Permissao.PFR_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromServices] IComandosFechamentoReabertura comandosFechamentoReabertura,
            [FromBody]FechamentoReaberturaAlteracaoDto fechamentoReaberturaPersistenciaDto, long id)
        {
            return Ok(await comandosFechamentoReabertura.Alterar(fechamentoReaberturaPersistenciaDto, id));
        }

        [HttpGet]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<FechamentoReaberturaListagemDto>), 200)]
        [Permissao(Permissao.PFR_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar([FromServices] IConsultasFechamentoReabertura consultasFechamentoReabertura, [FromQuery]FechamentoReaberturaFiltroDto fechamentoReaberturaFiltroDto)
        {
            return Ok(await consultasFechamentoReabertura.Listar(fechamentoReaberturaFiltroDto.TipoCalendarioId, fechamentoReaberturaFiltroDto.DreId, fechamentoReaberturaFiltroDto.UeId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(string), 200)]
        [Permissao(Permissao.PFR_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromServices] IComandosFechamentoReabertura comandosFechamentoReabertura, [FromBody]FechamentoReaberturaPersistenciaDto fechamentoReaberturaPersistenciaDto)
        {
            return Ok(await comandosFechamentoReabertura.Salvar(fechamentoReaberturaPersistenciaDto));
        }
    }
}