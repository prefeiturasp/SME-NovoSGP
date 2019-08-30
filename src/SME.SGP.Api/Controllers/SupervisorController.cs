using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/supervisores")]
    [ValidaDto]
    public class SupervisorController : ControllerBase
    {
        private readonly IConsultasCiclo consultasCiclo;

        public SupervisorController(IConsultasCiclo consultasCiclo)
        {
            this.consultasCiclo = consultasCiclo ?? throw new System.ArgumentNullException(nameof(consultasCiclo));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CicloDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Get()
        {
            return Ok(consultasCiclo.Listar(new List<int>()));
        }
    }
}