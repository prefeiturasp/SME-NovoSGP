using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.Consultas;
using SME.SGP.Dto;

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
        [Permissao(Policy = "Bearer")]
        public IActionResult Get(long codigoTipoCalendario, [FromServices]IConsultasPeriodoEscolar consultas)
        {
            var periodoEscolar = consultas.ObterPorTipoCalendario(codigoTipoCalendario);

            if (periodoEscolar == null)
                return NoContent();

            return Ok(periodoEscolar);
        }
    }
}