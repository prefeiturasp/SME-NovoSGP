using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/filtros")]
    [Authorize("Bearer")]
    public class FiltroRelatorioController : ControllerBase
    {
        [HttpGet("dres")]
        public async Task<IActionResult> ObterDresPorAbrangencia([FromServices] IObterFiltroRelatoriosDresPorAbrangenciaUseCase obterDresPorAbrangenciaFiltroRelatoriosUseCase)
        {
            return Ok(await obterDresPorAbrangenciaFiltroRelatoriosUseCase.Executar());
        }

        [HttpGet("dres/{codigoDre}/ues")]
        public async Task<IActionResult> ObterUesPorDreComAbrangencia(string codigoDre, [FromServices] IObterFiltroRelatoriosUesPorAbrangenciaUseCase obterFiltroRelatoriosUesPorAbrangenciaUseCase)
        {
            return Ok(await obterFiltroRelatoriosUesPorAbrangenciaUseCase.Executar(codigoDre));
        }

        [HttpGet("ues/{codigoUe}/modalidades")]
        public async Task<IActionResult> ObterModalidadesPorUe(string codigoUe, [FromServices] IObterFiltroRelatoriosModalidadesPorUeUseCase obterFiltroRelatoriosModalidadesPorUeUseCase)
        {
            return Ok(await obterFiltroRelatoriosModalidadesPorUeUseCase.Executar(codigoUe));
        }

        [HttpGet("ues/{codigoUe}/modalidades/{modalidade}/anos-escolares")]
        public async Task<IActionResult> ObterAnosEscolaresPorModalidadeUe(string codigoUe, Modalidade modalidade, [FromServices] IObterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase obterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase)
        {
            return Ok(await obterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase.Executar(codigoUe, modalidade));
        }

        [HttpGet("ues/{codigoUe}/anoletivo/{anoLetivo}/turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<OpcaoDropdownDto>), 200)]
        public async Task<IActionResult> ObterTurmasEscolaresPorUEAnoLetivoModalidadeSemestre(string codigoUe, int anoLetivo, [FromQuery]int semestre, [FromQuery]Modalidade modalidade, [FromServices]IObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase obterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase)
        {
            return Ok(await obterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase.Executar(codigoUe, anoLetivo, modalidade, semestre));
        }
    }
}