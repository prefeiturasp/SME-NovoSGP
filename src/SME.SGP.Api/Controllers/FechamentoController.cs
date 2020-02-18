using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/periodos/fechamentos/aberturas")]
    [Authorize("Bearer")]
    public class FechamentoController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(FechamentoDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PFA_C, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromQuery]FiltroFechamentoDto fechamentoDto, [FromServices] IConsultasFechamento consultasFechamento)
        {
            return Ok(await consultasFechamento.ObterPorTipoCalendarioDreEUe(fechamentoDto));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PFA_I, Permissao.PFA_A, Policy = "Bearer")]
        public async Task<IActionResult> Post([FromBody]FechamentoDto fechamentoDto, [FromServices] IComandosFechamento comandosFechamento)
        {
            await comandosFechamento.Salvar(fechamentoDto);
            return Ok();
        }

        [HttpGet("turmas/{turmaId}/bimestres/{bimestre}/aberto")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PFA_C, Policy = "Bearer")]
        public async Task<IActionResult> PeriodoTurmaAberto(string turmaId, int bimestre, [FromQuery] DateTime dataReferencia, [FromServices] IConsultasFechamento consultasFechamento)
        {
            return Ok(await consultasFechamento.TurmaEmPeriodoDeFechamento(turmaId, dataReferencia, bimestre));
        }

    }
}