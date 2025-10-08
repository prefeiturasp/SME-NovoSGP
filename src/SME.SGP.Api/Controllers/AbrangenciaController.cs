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
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/abrangencias/{consideraHistorico}")]
    [Authorize("Bearer")]
    public class AbrangenciaController : ControllerBase
    {
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IServicoAbrangencia servicoAbrangencia;

        public AbrangenciaController(IConsultasAbrangencia consultasAbrangencia, IServicoAbrangencia servicoAbrangencia)
        {
            this.consultasAbrangencia = consultasAbrangencia ??
               throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.servicoAbrangencia = servicoAbrangencia ??
               throw new ArgumentNullException(nameof(servicoAbrangencia));
        }

        private bool ConsideraHistorico
        {
            get
            {
                if (this.RouteData.NaoEhNulo() && this.RouteData.Values.NaoEhNulo())
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
        public async Task<IActionResult> ObterAbrangenciaAutoComplete(string filtro, [FromQuery]bool consideraAnosTurmasInfantil = false)
        {
            if (filtro.Length < 2)
                return StatusCode(204);

            var retorno = await consultasAbrangencia.ObterAbrangenciaPorfiltro(filtro, ConsideraHistorico, consideraAnosTurmasInfantil);
            if (retorno.Any())
                return Ok(retorno);
            else return StatusCode(204);
        }

        [HttpGet("ues/{codigoUe}/modalidades/{modalidade}/turmas/anos/{anoletivo}")]
        [ProducesResponseType(typeof(IEnumerable<OpcaoDropdownDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAnosLetivos(string codigoUe, int modalidade,int? anoletivo)
        {
            var retorno = (await consultasAbrangencia.ObterAnosTurmasPorUeModalidade(codigoUe, (Modalidade)modalidade, ConsideraHistorico,anoletivo));

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
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaDreRetornoDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterDres([FromServices] IObterAbrangenciaDresUseCase useCase, [FromQuery] Modalidade? modalidade, [FromQuery] int periodo = 0, [FromQuery] int anoLetivo = 0, [FromQuery]  string filtro = "")
        {
             if (filtro.Length < 3)
                filtro = "";

            var dres = await useCase.Executar(modalidade, periodo, ConsideraHistorico, anoLetivo, filtro);

            if (dres.Any())
                return Ok(dres.OrderBy(d => d.Nome));

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
        public async Task<IActionResult> ObterSemestres([FromQuery] FiltroSemestreDto filtroSemestreDto)
        {
            var retorno = await consultasAbrangencia
                .ObterSemestres(filtroSemestreDto.Modalidade, ConsideraHistorico, filtroSemestreDto.AnoLetivo, filtroSemestreDto.DreCodigo, filtroSemestreDto.UeCodigo);

            if ((retorno.EhNulo() || !retorno.Any()) && !ConsideraHistorico)
            {
                retorno = await consultasAbrangencia
                    .ObterSemestres(filtroSemestreDto.Modalidade, true, filtroSemestreDto.AnoLetivo, filtroSemestreDto.DreCodigo, filtroSemestreDto.UeCodigo);
            }

            if (!retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

        [HttpGet("dres/ues/{codigoUe}/turmas")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaTurmaRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTurmas([FromServices] IMediator mediator, string codigoUe, [FromQuery] Modalidade modalidade, int periodo = 0, [FromQuery] int anoLetivo = 0, [FromQuery] int[] tipos = null, [FromQuery] bool consideraNovosAnosInfantil = false)
        {
            var turmas = await mediator.Send(
                new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(codigoUe, modalidade,
                    periodo, ConsideraHistorico, anoLetivo, tipos, consideraNovosAnosInfantil)); 

            if (!turmas.Any())
                return NoContent();

            return Ok(turmas);
        }


        [HttpGet("dres/ues/{codigoUe}/turmas/disciplina/{codigoDisciplina}")]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaTurmaRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTurmasMesmoComponenteCurricular([FromServices] IMediator mediator,string codigoUe, string codigoDisciplina, bool turmasRegulares, [FromQuery] Modalidade modalidade, int periodo = 0, [FromQuery] int anoLetivo = 0, [FromQuery] int[] tipos = null, [FromQuery] bool consideraNovosAnosInfantil = false)
        {
            IEnumerable<AbrangenciaTurmaRetorno> turmas;

            if (!turmasRegulares)
            {
                turmas = await mediator.Send(
                    new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(codigoUe, modalidade,
                        periodo, ConsideraHistorico, anoLetivo, tipos, consideraNovosAnosInfantil)); 

                if ((turmas.EhNulo() || !turmas.Any()) && !ConsideraHistorico)
                    turmas = await mediator.Send(
                        new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(codigoUe,
                            modalidade, periodo, true, anoLetivo, tipos, consideraNovosAnosInfantil)); 
            }
            else
            {
                turmas = await consultasAbrangencia.ObterTurmasRegulares(codigoUe, modalidade, periodo, ConsideraHistorico, anoLetivo);
            }

            if (!turmas.Any())
                return NoContent();
            else
                turmas = await consultasAbrangencia.ObterTurmasAbrangenciaMesmoComponente(turmas, codigoDisciplina);

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
        public async Task<IActionResult> ObterUes([FromServices] IObterUEsPorDreUseCase useCase, string codigoDre, [FromQuery] Modalidade? modalidade, [FromQuery] int periodo = 0, [FromQuery] int anoLetivo = 0, [FromQuery] bool consideraNovasUEs = false, [FromQuery] bool filtrarTipoEscolaPorAnoLetivo = false, string filtro = "")
        {

            if (filtro.Length < 3)
                filtro = "";

            var dto = new UEsPorDreDto()
            {
                CodigoDre = codigoDre,
                Modalidade = modalidade,
                Periodo = periodo,
                AnoLetivo = anoLetivo,
                ConsideraNovasUEs = consideraNovasUEs,
                FiltrarTipoEscolaPorAnoLetivo = filtrarTipoEscolaPorAnoLetivo,
                Filtro = filtro,
                ConsideraHistorico = ConsideraHistorico
            };

            var ues = await useCase.Executar(dto);

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
        [HttpPost("/api/v1/abrangencias/sincronizar-abrangencia/{professorRf}/{AnoLetivo}/turmas-historicas")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> SincronizarAbrangenciaTurmasHistoricas(string professorRf, int anoLetivo)
        {
            return Ok(await servicoAbrangencia.SincronizarAbrangenciaHistorica(anoLetivo, professorRf));
        }

        [HttpGet("/api/v1/abrangencias/turmas/vigentes")]
        [ProducesResponseType(typeof(IEnumerable<TurmaNaoHistoricaDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterTurmasNaoHistoricas([FromServices] IObterTurmasNaoHistoricasUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpGet("/api/v1/abrangencias/usuarios/{login}/perfis/{perfil}/carregar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> CarregarAbrangencia(string login, Guid perfil, [FromServices] ICarregarAbrangenciaUsusarioUseCase useCase)
        {
            return Ok(await useCase.Executar(new UsuarioPerfilDto(login, perfil)));
        }
    }
}