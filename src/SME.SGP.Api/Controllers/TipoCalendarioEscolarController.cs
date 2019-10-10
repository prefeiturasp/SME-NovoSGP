using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/tipo-calendario-escolar")]
    [ValidaDto]
    public class TipoCalendarioEscolarController : ControllerBase
    {
        private readonly IConsultasTipoCalendarioEscolar consultas;
        public TipoCalendarioEscolarController(IConsultasTipoCalendarioEscolar consultas)
        {
            this.consultas = consultas ?? throw new System.ArgumentNullException(nameof(consultas));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TipoCalendarioEscolarDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.C_C, Policy = "Bearer")]
        public IActionResult Get()
        {
            return Ok(consultas.Listar());
        }
    }

}
