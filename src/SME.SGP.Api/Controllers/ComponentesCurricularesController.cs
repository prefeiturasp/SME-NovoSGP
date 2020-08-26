using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/componentes-curriculares")]
    [Authorize("Bearer")]
    public class ComponentesCurricularesController : ControllerBase
    {
        [HttpGet("ues/{codigoUe}/modalidades/{modalidade}/anos/{anoLetivo}/anos-escolares")]
        [ProducesResponseType(typeof(IEnumerable<ComponenteCurricularEol>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterDresAtribuicoes(string codigoUe, Modalidade modalidade, [FromQuery] string[] anosEscolares, int anoLetivo, [FromServices] IObterComponentesCurricularesPorAnoEscolarUseCase obterComponentesCurricularesPorAnoEscolarUseCase)
        {
            return Ok(await obterComponentesCurricularesPorAnoEscolarUseCase.Executar(codigoUe, modalidade, anoLetivo, anosEscolares));
        }

        [HttpPost("ues/{ueId}/turmas")]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Obter(string ueId, string[] turmas, [FromServices] IObterComponentesCurricularesPorTurmaECodigoUeUseCase obterComponentesCurricularesPorTurmaECodigoUeUseCase)
        {
            var filtro = new FiltroComponentesCurricularesPorTurmaECodigoUeDto { CodigoUe = ueId, CodigosDeTurmas = turmas };
            return Ok(await obterComponentesCurricularesPorTurmaECodigoUeUseCase.Executar(filtro));
        }
    }
}