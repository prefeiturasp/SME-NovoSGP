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
    [Route("api/v1/notificacoes")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class NotificacaoController : ControllerBase
    {
        private readonly IComandosNotificacao comandosNotificacao;
        private readonly IConsultasNotificacao consultasNotificacao;

        public NotificacaoController(IComandosNotificacao comandosNotificacao, IConsultasNotificacao consultasNotificacao)
        {
            this.comandosNotificacao = comandosNotificacao ?? throw new System.ArgumentNullException(nameof(comandosNotificacao));
            this.consultasNotificacao = consultasNotificacao ?? throw new System.ArgumentNullException(nameof(consultasNotificacao));
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.N_E, Policy = "Bearer")]
        public IActionResult Delete(long[] notificacoesId)
        {
            return Ok(comandosNotificacao.Excluir(notificacoesId));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<NotificacaoBasicaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.N_C, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromQuery]NotificacaoFiltroDto notificacaoFiltroDto)
        {
            return Ok(await consultasNotificacao.Listar(notificacaoFiltroDto));
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(NotificacaoDetalheDto), 500)]
        [Route("{notificacaoId}")]
        [Permissao(Permissao.N_C, Policy = "Bearer")]
        public IActionResult Get(long notificacaoId)
        {
            return Ok(consultasNotificacao.Obter(notificacaoId));
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("status/lida")]
        [Permissao(Permissao.N_A, Policy = "Bearer")]
        public IActionResult MarcarComoLida(IList<long> notificaoesId)
        {
            return Ok(comandosNotificacao.MarcarComoLida(notificaoesId));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("categorias")]
        [Permissao(Permissao.N_C, Policy = "Bearer")]
        public IActionResult ObtemCategorias()
        {
            return Ok(consultasNotificacao.ObterCategorias());
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("status")]
        [Permissao(Permissao.N_C, Policy = "Bearer")]
        public IActionResult ObtemStatus()
        {
            return Ok(consultasNotificacao.ObterStatus());
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("tipos")]
        [Permissao(Permissao.N_C, Policy = "Bearer")]
        public IActionResult ObtemTipos()
        {
            return Ok(consultasNotificacao.ObterTipos());
        }

        [HttpGet]
        [ProducesResponseType(typeof(NotificacaoBasicaListaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("resumo")]
        [Permissao(Permissao.N_C, Policy = "Bearer")]
        public IActionResult ObtenhaPorRFAnoLetivo(int anoLetivo, string usuarioRf)
        {
            return Ok(consultasNotificacao.ObterNotificacaoBasicaLista(anoLetivo, usuarioRf));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.N_I, Policy = "Bearer")]
        public IActionResult Post([FromBody]NotificacaoDto notificacaoDto)
        {
            comandosNotificacao.Salvar(notificacaoDto);
            return Ok();
        }


        [HttpGet]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("nao-lidas/quantidade")]
        [Permissao(Permissao.N_C, Policy = "Bearer")]
        public async Task<IActionResult> ObtemQuantidadeNaoLida([FromServices] IObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpGet]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("nao-lidas")]
        [Permissao(Permissao.N_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterUltimasNaoLidas([FromServices] IObterUltimasNotificacoesNaoLidasPorUsuarioUseCase useCase, [FromQuery]bool tituloReduzido = false)
        {
            return Ok(await useCase.Executar(tituloReduzido));
        }
    }
}