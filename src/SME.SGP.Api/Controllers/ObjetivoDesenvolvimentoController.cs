using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

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
        [ProducesResponseType(typeof(IEnumerable<ObjetivoDesenvolvimentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDC_C, Policy = "Bearer")]
        public IActionResult Get()
        {
            return Ok(consultasObjetivoDesenvolvimento.Listar());
        }
    }
}