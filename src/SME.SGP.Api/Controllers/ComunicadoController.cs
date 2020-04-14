using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/comunicado")]
    [Authorize("Bearer")]
    public class ComunicadoController : ControllerBase
    {
        private readonly IComandoComunicado comandos;
        private readonly IConsultaComunicado consultas;

        public ComunicadoController(IConsultaComunicado consultas, IComandoComunicado comandos)
        {
            this.consultas = consultas ?? throw new System.ArgumentNullException(nameof(consultas));
            this.comandos = comandos ?? throw new System.ArgumentNullException(nameof(comandos));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar(long id, [FromBody]ComunicadoInserirDto comunicadoDto)
        {
            return Ok(await comandos.Alterar(id, comunicadoDto));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ComunicadoCompletoDto), 200)]
        [ProducesResponseType(typeof(IEnumerable<ComunicadoCompletoDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarPorId(long id)
        {
            return Ok(await consultas.BuscarPorIdAsync(id));
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(IEnumerable<ComunicadoCompletoDto>), 200)]
        [ProducesResponseType(typeof(IEnumerable<ComunicadoCompletoDto>), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarTodosAsync([FromQuery]FiltroComunicadoDto filtro)
        {
            var resultado = await consultas.ListarPaginado(filtro);
            if (!resultado.Items.Any())
                return NoContent();
            return Ok(resultado);
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CO_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir([FromBody]long[] ids)
        {
            await comandos.Excluir(ids);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CO_I, Policy = "Bearer")]
        public async Task<IActionResult> PostAsync([FromBody]ComunicadoInserirDto comunicadoDto)
        {
            return Ok(await comandos.Inserir(comunicadoDto));
        }
    }
}