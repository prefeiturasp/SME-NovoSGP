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
            [FromBody]FechamentoReaberturaAlteracaoDto fechamentoReaberturaPersistenciaDto, long id, [FromQuery]bool AlteracaoHierarquicaConfirmacao = false)
        {
            return Ok(await comandosFechamentoReabertura.Alterar(fechamentoReaberturaPersistenciaDto, id, AlteracaoHierarquicaConfirmacao));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(string), 200)]
        [Permissao(Permissao.PFR_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir([FromServices] IComandosFechamentoReabertura comandosFechamentoReabertura, [FromBody]long[] ids)
        {
            return Ok(await comandosFechamentoReabertura.Excluir(ids));
        }

        [HttpGet]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<FechamentoReaberturaListagemDto>), 200)]
        [Permissao(Permissao.PFR_C, Policy = "Bearer")]
        public async Task<IActionResult> Listar([FromServices] IConsultasFechamentoReabertura consultasFechamentoReabertura, [FromQuery]FechamentoReaberturaFiltroDto fechamentoReaberturaFiltroDto)
        {
            return Ok(await consultasFechamentoReabertura.Listar(fechamentoReaberturaFiltroDto.TipoCalendarioId, fechamentoReaberturaFiltroDto.DreCodigo, fechamentoReaberturaFiltroDto.UeCodigo, fechamentoReaberturaFiltroDto.AprovadorCodigo));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(FechamentoReaberturaRetornoDto), 200)]
        [Permissao(Permissao.PFR_C, Policy = "Bearer")]
        public IActionResult ObterPorId([FromServices] IConsultasFechamentoReabertura consultasFechamentoReabertura, long id)
        {
            return Ok(consultasFechamentoReabertura.ObterPorId(id));
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