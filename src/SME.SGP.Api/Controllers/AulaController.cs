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
        public async Task<IActionResult> BuscarPorId(long id, [FromServices]IConsultasAula consultas)
        {
            var aula = await consultas.BuscarPorId(id);
            return Ok(aula);
        }

        [HttpDelete("{id}/recorrencias/{recorrencia}/disciplinaNome/{disciplinaNome}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir(long id, string disciplinaNome, RecorrenciaAula recorrencia, [FromServices]IComandosAula comandos)
        {
            var retorno = new RetornoBaseDto();
            retorno.Mensagens.Add(await comandos.Excluir(id, disciplinaNome, recorrencia));
            return Ok(retorno);
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

        [HttpGet("{aulaId}/recorrencias/serie")]
        [ProducesResponseType(typeof(AulaRecorrenciaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterRecorrenciaDaSerie(long aulaId, [FromServices]IConsultasAula consultas)
        {
            var recorrencia = await consultas.ObterRecorrenciaDaSerie(aulaId);
            var quantidadeAulas = recorrencia == (int)RecorrenciaAula.AulaUnica ? 1
                : await consultas.ObterQuantidadeAulasRecorrentes(aulaId, RecorrenciaAula.RepetirTodosBimestres);
            var existeFrequenciaPlanoAula = await consultas.ChecarFrequenciaPlanoNaRecorrencia(aulaId);

            return Ok(new AulaRecorrenciaDto()
            {
                AulaId = aulaId,
                RecorrenciaAula = recorrencia,
                QuantidadeAulasRecorrentes = quantidadeAulas,
                ExisteFrequenciaOuPlanoAula = existeFrequenciaPlanoAula
            });
        }
    }
}