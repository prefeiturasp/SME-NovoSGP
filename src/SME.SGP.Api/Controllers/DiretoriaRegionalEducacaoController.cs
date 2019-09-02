using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/dres")]
    public class DiretoriaRegionalEducacaoController : ControllerBase
    {
        private readonly IConsultaDres consultaDres;

        public DiretoriaRegionalEducacaoController(IConsultaDres consultaDres)
        {
            this.consultaDres = consultaDres ?? throw new System.ArgumentNullException(nameof(consultaDres));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CicloDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Get()
        {
            return Ok(consultaDres.ObterTodos());
        }

        [HttpGet("{dreId}/escolas/sem-atribuicao")]
        [ProducesResponseType(typeof(IEnumerable<UnidadeEscolarDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult ObterEscolasSemAtribuicao(string dreId)
        {
            return Ok(consultaDres.ObterEscolasSemAtribuicao(dreId));
        }
    }
}