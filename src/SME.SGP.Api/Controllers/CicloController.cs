using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;

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
        [ProducesResponseType(typeof(CicloDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Get(int ano)
        {
            return Ok(consultasCiclo.Selecionar(ano));
        }
    }
}