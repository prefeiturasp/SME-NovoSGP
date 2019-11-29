using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
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
            var retorno = new RetornoBaseDto();
            retorno.Mensagens.Add(await comandos.Alterar(dto, id));
            return Ok(retorno);
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

        [HttpDelete("{id}/recorrencia/{recorrencia}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_E, Policy = "Bearer")]
        public IActionResult Excluir(long id, RecorrenciaAula recorrencia, [FromServices]IComandosAula comandos)
        {
            comandos.Excluir(id, recorrencia);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> Inserir([FromBody]AulaDto dto, [FromServices]IComandosAula comandos)
        {
            var retorno = new RetornoBaseDto();
            retorno.Mensagens.Add(await comandos.Inserir(dto));
            return Ok(retorno);
        }

        [HttpGet("{aulaId}/recorrencias/{recorrencia}/quantidade")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterQuantidadeAulasRecorrencia(long aulaId, RecorrenciaAula recorrencia, [FromServices]IConsultasAula consultas)
        {
            return Ok(await consultas.ObterQuantidadeAulasRecorrentes(aulaId, recorrencia));
        }

        [HttpGet("{aulaId}/recorrencias/serie")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterRecorrenciaDaSerie(long aulaId, [FromServices]IConsultasAula consultas)
        {
            var recorrencia = await consultas.ObterRecorrenciaDaSerie(aulaId);

            return Ok(recorrencia);
        }
    }
}