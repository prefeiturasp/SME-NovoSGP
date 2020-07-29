using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
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
        public async Task<IActionResult> ObterAnosLetivos()
        {
            int[] retorno = (await consultasAbrangencia.ObterAnosLetivos(ConsideraHistorico)).ToArray();

            if (!retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("anos-letivos-todos")]
        [ProducesResponseType(typeof(int[]), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAnosLetivosTodos()
        {
            int[] retorno = (await consultasAbrangencia.ObterAnosLetivosTodos()).ToArray();

            if (!retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("dres")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaDreRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDres([FromQuery]Modalidade? modalidade, [FromQuery]int periodo = 0, [FromQuery]int anoLetivo = 0)
        {
            var consideraHistorico = ((anoLetivo != 0) && (anoLetivo < DateTime.Now.Year))
                                    || ConsideraHistorico;

            var dres = await consultasAbrangencia.ObterDres(modalidade, periodo, consideraHistorico, anoLetivo);

            if (dres.Any())
                return Ok(dres);

            return StatusCode(204);
        }

        [HttpGet("modalidades")]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterModalidades(int anoLetivo)
        {
            var retorno = await consultasAbrangencia.ObterModalidades(anoLetivo, ConsideraHistorico);

            if (!retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("semestres")]
        [ProducesResponseType(typeof(int[]), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSemestres([FromQuery]Modalidade modalidade, [FromQuery]int anoLetivo = 0)
        {
            var retorno = await consultasAbrangencia.ObterSemestres(modalidade, ConsideraHistorico, anoLetivo);

            if (!retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("dres/ues/{codigoUe}/turmas")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaTurmaRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTurmas(string codigoUe, [FromQuery]Modalidade modalidade, int periodo = 0, [FromQuery]int anoLetivo = 0)
        {
            var consideraHistorico = ((anoLetivo != 0) && (anoLetivo < DateTime.Now.Year))
                                    || ConsideraHistorico;

            var turmas = await consultasAbrangencia.ObterTurmas(codigoUe, modalidade, periodo, consideraHistorico, anoLetivo);

            if (!turmas.Any())
                return NoContent();

            return Ok(turmas);
        }

        [HttpGet("dres/{codigoDre}/ues")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaUeRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterUes(string codigoDre, [FromQuery]Modalidade? modalidade, [FromQuery]int periodo = 0, [FromQuery]int anoLetivo = 0)
        {
            var consideraHistorico = ((anoLetivo != 0) && (anoLetivo < DateTime.Now.Year))
                                    || ConsideraHistorico;

            var ues = await consultasAbrangencia.ObterUes(codigoDre, modalidade, periodo, consideraHistorico, anoLetivo);

            if (!ues.Any())
                return NoContent();

            return Ok(ues);
        }

        [HttpGet("adm")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> UsuarioAdm([FromServices] IUsuarioPossuiAbrangenciaAdmUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

    }
}