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
    //[Authorize("Bearer")]
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

        [HttpGet("ues/{codigoUe}/modalidades/{modalidade}/turmas/anos")]
        [ProducesResponseType(typeof(IEnumerable<OpcaoDropdownDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAnosLetivos(string codigoUe, int modalidade)
        {
            var retorno = (await consultasAbrangencia.ObterAnosTurmasPorUeModalidade(codigoUe, (Modalidade)modalidade, ConsideraHistorico));

            if (!retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("anos-letivos")]
        [ProducesResponseType(typeof(int[]), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAnosLetivos([FromQuery] int anoMinimo)
        {
            int[] retorno = (await consultasAbrangencia.ObterAnosLetivos(ConsideraHistorico, anoMinimo)).ToArray();

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
        public async Task<IActionResult> ObterDres([FromQuery] Modalidade? modalidade, [FromQuery] int periodo = 0, [FromQuery] int anoLetivo = 0, [FromQuery]  string filtro = "")
        {
             if (filtro.Length < 3)
                filtro = "";

            var dres = await consultasAbrangencia.ObterDres(modalidade, periodo, ConsideraHistorico, anoLetivo, filtro);

            if (dres.Any())
                return Ok(dres);

            return StatusCode(204);
        }

        [HttpGet("modalidades")]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterModalidades([FromServices] IObterModalidadesPorAnoUseCase obterModalidadesPorAnoUseCase, int anoLetivo, bool consideraNovasModalidades = false)
        {
            var retorno = await obterModalidadesPorAnoUseCase.Executar(anoLetivo, ConsideraHistorico, consideraNovasModalidades);
            if (!retorno?.Any() ?? true)
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("semestres")]
        [ProducesResponseType(typeof(int[]), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSemestres([FromQuery] Modalidade modalidade, [FromQuery] int anoLetivo = 0)
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
        public async Task<IActionResult> ObterTurmas(string codigoUe, [FromQuery] Modalidade modalidade, int periodo = 0, [FromQuery] int anoLetivo = 0, [FromQuery] int[] tipos = null)
        {
            IEnumerable<AbrangenciaTurmaRetorno> turmas;
            turmas = await consultasAbrangencia.ObterTurmas(codigoUe, modalidade, periodo, ConsideraHistorico, anoLetivo, tipos);

            if (!turmas.Any())
                return NoContent();

            return Ok(turmas);
        }


        [HttpGet("dres/ues/{codigoUe}/turmas-regulares")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaTurmaRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTurmasRegulares(string codigoUe, [FromQuery] Modalidade modalidade, int periodo = 0, [FromQuery] int anoLetivo = 0)
        {
            var turmas = await consultasAbrangencia.ObterTurmasRegulares(codigoUe, modalidade, periodo, ConsideraHistorico, anoLetivo);

            if (!turmas.Any())
                return NoContent();

            return Ok(turmas);
        }

        [HttpGet("dres/{codigoDre}/ues")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaUeRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterUes([FromServices] IObterUEsPorDreUseCase useCase, string codigoDre, [FromQuery] Modalidade? modalidade, [FromQuery] int periodo = 0, [FromQuery] int anoLetivo = 0, [FromQuery] bool consideraNovasUEs = false)
        {
            var ues = await useCase.Executar(codigoDre, modalidade, periodo, ConsideraHistorico, anoLetivo, consideraNovasUEs);

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
        [HttpGet("/api/v1/abrangencias/{usuarioRF}/perfis/{usuarioPerfil}/acesso-sondagem")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [AllowAnonymous]
        public async Task<IActionResult> PodeAcessarSondagem(string usuarioRF, Guid usuarioPerfil, [FromServices] IUsuarioPossuiAbrangenciaAcessoSondagemUseCase useCase)
        {
            return Ok(await useCase.Executar(usuarioRF, usuarioPerfil));
        }

    }
}