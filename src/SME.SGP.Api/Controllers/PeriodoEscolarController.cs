using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/periodo-escolar")]
    [ValidaDto]
    public class PeriodoEscolarController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(PeriodoEscolarDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PE_C, Policy = "Bearer")]
        public async Task<IActionResult> Get(long codigoTipoCalendario, [FromServices] IConsultasPeriodoEscolar consultas)
        {
            var periodoEscolar = await consultas.ObterPorTipoCalendario(codigoTipoCalendario);

            if (periodoEscolar == null)
                return NoContent();

            return Ok(periodoEscolar);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PE_I, Permissao.PE_A, Policy = "Bearer")]
        public async Task<IActionResult> Post([FromBody] PeriodoEscolarListaDto periodos, [FromServices] IComandosPeriodoEscolar comandoPeriodo)
        {
            await comandoPeriodo.Salvar(periodos);
            return Ok();
        }

        [HttpGet("bimestres/{bimestre}/turmas/{turmaCodigo}/aberto")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> PeriodoEmAberto(string turmaCodigo, int bimestre, [FromQuery] DateTime dataReferencia, [FromServices] IConsultasTurma consultas)
        {
            var dataConsulta = dataReferencia == DateTime.MinValue ? DateTime.Today : dataReferencia;
            return Ok(await consultas.TurmaEmPeriodoAberto(turmaCodigo, dataConsulta, bimestre));
        }

        [HttpGet("turmas/{turmaCodigo}/bimestres/aberto")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IEnumerable<PeriodoEscolarAbertoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> PeriodosEmAberto(string turmaCodigo, [FromQuery] DateTime dataReferencia, [FromServices] IConsultasTurma consultas)
        {
            var dataConsulta = dataReferencia == DateTime.MinValue ? DateTime.Today : dataReferencia;
            return Ok(await consultas.PeriodosEmAbertoTurma(turmaCodigo, dataConsulta));
        }

        [HttpGet("modalidades/{modalidade}/bimestres/atual")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAtual(int modalidade, [FromServices] IConsultasPeriodoEscolar consultas)
        {
            return Ok(await consultas.ObterBimestre(DateTime.Today, (Dominio.Modalidade)modalidade));
        }
    }
}