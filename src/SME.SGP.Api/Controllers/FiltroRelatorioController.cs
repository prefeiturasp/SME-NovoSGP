﻿using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("ues/{codigoUe}/modalidades/abrangencias")]
        public async Task<IActionResult> ObterModalidadesPorUeAbrangencia(string codigoUe, [FromServices] IObterFiltroRelatoriosModalidadesPorUeAbrangenciaUseCase obterFiltroRelatoriosModalidadesPorUeAbrangenciaUseCase)
        {
            return Ok(await obterFiltroRelatoriosModalidadesPorUeAbrangenciaUseCase.Executar(codigoUe));
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

        [HttpGet("turmas/ues/{codigoUe}/anoletivo/{anoLetivo}")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(IEnumerable<OpcaoDropdownDto>), 200)]
        public async Task<IActionResult> ObterTurmasEscolaresPorUEAnoLetivoModalidadeSemestreEAno(string codigoUe, int anoLetivo, [FromQuery] int semestre, [FromQuery] Modalidade modalidade, [FromQuery] IList<string> anos, [FromServices] IObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase obterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase)
        {
            return Ok(await obterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase.Executar(codigoUe, anoLetivo, modalidade, semestre, anos));
        }

        [HttpGet("ues/{codigoUe}/modalidades/{modalidade}/ciclos")]
        [ProducesResponseType(typeof(IEnumerable<RetornoCicloDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterCiclosPorModalidadeECodigoUe(int modalidade, string codigoUe, [FromServices] IObterCiclosPorModalidadeECodigoUeUseCase obterCiclosPorModalidadeECodigoUeUseCase, [FromQuery]bool consideraAbrangencia = false)
        {
            return Ok(await obterCiclosPorModalidadeECodigoUeUseCase.Executar(new FiltroCicloPorModalidadeECodigoUeDto(modalidade, codigoUe, consideraAbrangencia)));
        }

        [HttpGet("modalidades/{modalidade}/ciclos/{cicloId}/anos-escolares")]
        [ProducesResponseType(typeof(IEnumerable<RetornoCicloDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterAnosPorCicloId(long cicloId, Modalidade modalidade, [FromServices] IObterFiltroRelatoriosAnosPorCicloModalidadeUseCase obterFiltroRelatoriosAnosPorCicloModalidadeUseCase)
        {
            return Ok(await obterFiltroRelatoriosAnosPorCicloModalidadeUseCase.Executar(cicloId, modalidade));
        }
        [HttpGet("componentes-curriculares/anos-letivos/{anoLetivo}/ues/{codigoUe}/modalidades/{modalideId}")]
        [ProducesResponseType(typeof(IEnumerable<RetornoCicloDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterComponentesCurricularesPorAnoUeModalidade([FromQuery]string[] anos, int anoLetivo,  string codigoUe, Modalidade modalideId, [FromServices] IObterComponentesCurricularesPorUeAnosModalidadeUseCase obterComponentesCurricularesPorUeAnosModalidadeUseCase)
        {
            return Ok(await obterComponentesCurricularesPorUeAnosModalidadeUseCase.Executar(anos, anoLetivo, codigoUe, modalideId));
        }
    }
}