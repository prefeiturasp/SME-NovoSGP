using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/aula-prevista")]
    [ValidaDto]
    public class AulaPrevistaController : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AulasPrevistasDadasAuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.ADAP_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarPorId(long id, [FromServices]IConsultasAulaPrevista consultas)
        {
            return Ok(await consultas.BuscarPorId(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ADAP_I, Policy = "Bearer")]
        public async Task<IActionResult> Inserir([FromBody]AulaPrevistaDto dto, [FromServices]IComandosAulaPrevista comandos)
        {
            return Ok(await comandos.Inserir(dto));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody]AulaPrevistaDto dto, long id, [FromServices]IComandosAulaPrevista comandos)
        {
            var retorno = new RetornoBaseDto();
            retorno.Mensagens.Add(await comandos.Alterar(dto, id));
            return Ok(retorno);
        }

        [HttpPost("notificar")]
        public async Task<IActionResult> Notificar([FromServices] IServicoNotificacaoAulaPrevista servicoNotificacaoAulaPrevista)
        {
            servicoNotificacaoAulaPrevista.ExecutaNotificacaoAulaPrevista();
            return Ok();
        }
    }
}
