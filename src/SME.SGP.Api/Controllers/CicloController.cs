using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ciclos")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class CicloController : ControllerBase
    {
        private readonly IConsultasCiclo consultasCiclo;

        public CicloController(IConsultasCiclo consultasCiclo)
        {
            this.consultasCiclo = consultasCiclo ?? throw new System.ArgumentNullException(nameof(consultasCiclo));
        }

        [HttpPost("filtro")]
        [ProducesResponseType(typeof(IEnumerable<CicloDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Filtrar(FiltroCicloDto filtroCicloDto)
        {
            var retorno = consultasCiclo.Listar(filtroCicloDto);

            if (retorno == null || !retorno.Any())
                return NoContent();

            return Ok(retorno);
        }
    }
}