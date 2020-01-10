using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/abrangencias/{consideraHistorico}")]
    [Authorize("Bearer")]
    public class AbrangenciaController : ControllerBase
    {
        private readonly IConsultasAbrangencia consultasAbrangencia;

        public AbrangenciaController(IConsultasAbrangencia consultasAbrangencia)
        {
            this.consultasAbrangencia = consultasAbrangencia ??
               throw new System.ArgumentNullException(nameof(consultasAbrangencia));
        }

        private bool ConsideraHistorico
        {
            get
            {
                if (this.RouteData != null && this.RouteData.Values != null)
                {
                    var consideraHistoricoParam = (string)this.RouteData.Values["consideraHistorico"];

                    if (!string.IsNullOrWhiteSpace(consideraHistoricoParam))
                        return bool.Parse(consideraHistoricoParam);
                }

                return false;
            }
        }

        [HttpGet("{filtro}")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaFiltroRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAbrangenciaAutoComplete(string filtro)
        {
            if (filtro.Length < 2)
                return StatusCode(204);

            var retorno = await consultasAbrangencia.ObterAbrangenciaPorfiltro(filtro, ConsideraHistorico);
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }

        [HttpGet("anos-letivos")]
        [ProducesResponseType(typeof(int[]), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult ObterAnosLetivos()
        {
            int[] retorno = consultasAbrangencia.ObterAnosLetivos(ConsideraHistorico).Result.ToArray();
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }

        [HttpGet("dres")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaDreRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDres([FromQuery]Modalidade? modalidade, [FromQuery]int periodo = 0)
        {
            var dres = await consultasAbrangencia.ObterDres(modalidade, periodo, ConsideraHistorico);

            if (dres.Any())
                return Ok(dres);

            return StatusCode(204);
        }

        [HttpGet("modalidades")]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterModalidades(int anoLetivo)
        {
            var retorno = await consultasAbrangencia.ObterModalidades(anoLetivo, ConsideraHistorico);

            if (retorno.Any())
                return Ok(retorno);

            return NoContent();
        }

        [HttpGet("semestres")]
        [ProducesResponseType(typeof(int[]), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSemestres([FromQuery]Modalidade modalidade)
        {
            var retorno = await consultasAbrangencia.ObterSemestres(modalidade, ConsideraHistorico);
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }

        [HttpGet("dres/ues/{codigoUe}/turmas")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaTurmaRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTurmas(string codigoUe, [FromQuery]Modalidade modalidade, int periodo = 0)
        {
            var turmas = await consultasAbrangencia.ObterTurmas(codigoUe, modalidade, periodo, ConsideraHistorico);
            if (turmas.Any())
                return Ok(turmas);
            else return StatusCode(204);
        }

        [HttpGet("dres/{codigoDre}/ues")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaUeRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterUes(string codigoDre, [FromQuery]Modalidade? modalidade, [FromQuery]int periodo = 0)
        {
            var ues = await consultasAbrangencia.ObterUes(codigoDre, modalidade, periodo, ConsideraHistorico);

            if (ues.Any())
                return Ok(ues);

            return StatusCode(204);
        }
    }
}