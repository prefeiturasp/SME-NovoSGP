using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/matrizes-saber")]
    [ValidaDto]
    public class MatrizSaberController : ControllerBase
    {
        private readonly IConsultasMatrizSaber consultasMatrizSaber;

        public MatrizSaberController(IConsultasMatrizSaber consultasMatrizSaber)
        {
            this.consultasMatrizSaber = consultasMatrizSaber ?? throw new System.ArgumentNullException(nameof(consultasMatrizSaber));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MatrizSaberDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public IActionResult Get()
        {
            return Ok(consultasMatrizSaber.Listar());
        }
    }
}