using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/objetivos-desenvolvimento-sustentavel")]
    [ValidaDto]
    public class ObjetivoDesenvolvimentoController : ControllerBase
    {
        private readonly IConsultasObjetivoDesenvolvimento consultasObjetivoDesenvolvimento;

        public ObjetivoDesenvolvimentoController(IConsultasObjetivoDesenvolvimento consultasObjetivoDesenvolvimento)
        {
            this.consultasObjetivoDesenvolvimento = consultasObjetivoDesenvolvimento ?? throw new System.ArgumentNullException(nameof(consultasObjetivoDesenvolvimento));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(consultasObjetivoDesenvolvimento.Listar());
        }
    }
}