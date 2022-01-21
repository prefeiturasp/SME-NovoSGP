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
    public class PeriodoFechamentoController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(FechamentoDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PFA_C, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromQuery]FiltroFechamentoDto fechamentoDto, [FromServices] IConsultasPeriodoFechamento consultasFechamento)
        {
            return Ok(await consultasFechamento.ObterPorTipoCalendarioSme(fechamentoDto));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PFA_I, Permissao.PFA_A, Policy = "Bearer")]
        public async Task<IActionResult> Post([FromBody]FechamentoDto fechamentoDto, [FromServices] IComandosPeriodoFechamento comandosFechamento)
        {
            await comandosFechamento.Salvar(fechamentoDto);
            return Ok();
        }

        [HttpGet("turmas/{turmaCodigo}/bimestres/{bimestre}/aberto")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PFA_C, Policy = "Bearer")]
        public async Task<IActionResult> PeriodoTurmaAberto(string turmaCodigo, int bimestre, [FromQuery] DateTime dataReferencia, [FromServices] IConsultasPeriodoFechamento consultasFechamento)
        {
            if (dataReferencia == DateTime.MinValue)
                dataReferencia = DateTime.Now;
            return Ok(await consultasFechamento.TurmaEmPeriodoDeFechamento(turmaCodigo, dataReferencia, bimestre));
        }

    }
}