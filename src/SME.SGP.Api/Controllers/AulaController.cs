using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/professores/aulas")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class AulaController : ControllerBase
    {
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody]AulaDto dto, long id, [FromServices]IComandosAula comandos)
        {
            await comandos.Alterar(dto, id);
            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AulaConsultaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public IActionResult BuscarPorId(long id, [FromServices]IConsultasAula consultas)
        {
            var aula = consultas.BuscarPorId(id);
            return Ok(aula);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_E, Policy = "Bearer")]
        public IActionResult Excluir(long id, [FromServices]IComandosAula comandos)
        {
            comandos.Excluir(id);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> Inserir([FromBody]AulaDto dto, [FromServices]IComandosAula comandos)
        {
            await comandos.Inserir(dto);
            return Ok();
        }
    }
}