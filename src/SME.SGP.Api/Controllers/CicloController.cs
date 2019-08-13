using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ciclos")]
    [ValidaDto]
    public class CicloController : ControllerBase
    {
        private readonly IConsultasCiclo consultasCiclo;

        public CicloController(IConsultasCiclo consultasCiclo)
        {
            this.consultasCiclo = consultasCiclo ?? throw new System.ArgumentNullException(nameof(consultasCiclo));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(consultasCiclo.Listar(new List<int>()));
        }
    }
}