using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/professores/aulas")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class AulaController : ControllerBase
    {
        private readonly IComandosAula comandos;
        private readonly IConsultasAula consultas;

        public AulaController(IComandosAula comandos)
        {
            this.comandos = comandos ?? throw new System.ArgumentNullException(nameof(comandos));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.CP_I, Permissao.CP_A, Policy = "Bearer")]
        public async Task<IActionResult> Inserir([FromBody]AulaDto dto)
        {
            await comandos.Inserir(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_I, Permissao.CP_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody]AulaDto dto, long id)
        {
            await comandos.Alterar(dto, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_I, Permissao.CP_A, Policy = "Bearer")]
        public IActionResult Excluir(long id)
        {
            comandos.Excluir(id);
            return Ok();
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(IEnumerable<AulaConsultaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.CP_C, Policy = "Bearer")]
        public IActionResult Listar([FromQuery]FiltroAulaDto filtro)
        {
            var lista = consultas.Listar(filtro);
            return Ok(lista);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AulaConsultaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.CP_C, Policy = "Bearer")]
        public IActionResult BuscarPorId(long id)
        {
            var aula = consultas.BuscarPorId(id);
            return Ok(aula);
        }
    }
}
