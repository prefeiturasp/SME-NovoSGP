using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/feriado-calendario")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class FeriadoCalendarioController : ControllerBase
    {
        private readonly IComandosFeriadoCalendario comandos;
        private readonly IConsultasFeriadoCalendario consultas;

        public FeriadoCalendarioController(IConsultasFeriadoCalendario consultas,
            IComandosFeriadoCalendario comandos)
        {
            this.consultas = consultas ?? throw new System.ArgumentNullException(nameof(consultas));
            this.comandos = comandos ?? throw new System.ArgumentNullException(nameof(comandos));
        }

        [HttpGet]
        [ProducesResponseType(typeof(FeriadoCalendarioDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("{id}")]
        public IActionResult BuscarPorId(long id)
        {
            return Ok(consultas.BuscarPorId(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<FeriadoCalendarioDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("feriados")]
        public IActionResult BuscarTodos([FromBody] FiltroFeriadoCalendarioDto filtro)
        {
            return Ok(consultas.Listar(filtro));
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult MarcarExcluidos([FromBody]long[] ids)
        {
            comandos.MarcarExcluidos(ids);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Salvar([FromBody]FeriadoCalendarioDto dto)
        {
            comandos.Salvar(dto);
            return Ok();
        }
    }
}