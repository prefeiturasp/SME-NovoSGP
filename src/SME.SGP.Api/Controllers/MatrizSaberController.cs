using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;

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
        public IActionResult Get()
        {
            return Ok(consultasMatrizSaber.Listar());
        }
    }
}