using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicao/poa")]
    [ValidaDto]
    public class RegistroPoaController : ControllerBase
    {
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public IActionResult Put(long id,[FromBody]RegistroPoaDto registroPoaDto, [FromServices]IComandosRegistroPoa comandosRegistroPoa)
        {
            registroPoaDto.Id = id;

            comandosRegistroPoa.Atualizar(registroPoaDto);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public IActionResult Post([FromBody]RegistroPoaDto registroPoaDto, [FromServices]IComandosRegistroPoa comandosRegistroPoa)
        {
            comandosRegistroPoa.Cadastrar(registroPoaDto);

            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RegistroPoaCompletoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public IActionResult Get(long id, [FromServices]IConsultasRegistroPoa consultasRegistroPoa)
        {
            var retorno = consultasRegistroPoa.ObterPorId(id);

            if (retorno != null)
                return Ok(retorno);

            return NoContent();
        }
    }
}