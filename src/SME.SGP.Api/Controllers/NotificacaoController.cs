using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/notificacoes")]
    [ValidaDto]
    public class NotificacaoController : ControllerBase
    {
        private readonly IComandosNotificacao comandosNotificacao;
        private readonly IConsultasNotificacao consultasNotificacao;

        public NotificacaoController(IComandosNotificacao comandosNotificacao, IConsultasNotificacao consultasNotificacao)
        {
            this.comandosNotificacao = comandosNotificacao ?? throw new System.ArgumentNullException(nameof(comandosNotificacao));
            this.consultasNotificacao = consultasNotificacao ?? throw new System.ArgumentNullException(nameof(consultasNotificacao));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NotificacaoBasicaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Get([FromQuery]NotificacaoFiltroDto notificacaoFiltroDto)
        {
            return Ok(consultasNotificacao.Listar(notificacaoFiltroDto));
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(NotificacaoDetalheDto), 500)]
        [Route("{notificacaoId}")]
        public IActionResult Get(long notificacaoId)
        {
            return Ok(consultasNotificacao.Obter(notificacaoId));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("categorias")]
        public IActionResult ObtemCategorias()
        {
            return Ok(consultasNotificacao.ObterCategorias());
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("status")]
        public IActionResult ObtemStatus()
        {
            return Ok(consultasNotificacao.ObterStatus());
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnumeradoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("tipos")]
        public IActionResult ObtemTipos()
        {
            return Ok(consultasNotificacao.ObterTipos());
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Post([FromBody]NotificacaoDto notificacaoDto)
        {
            comandosNotificacao.Salvar(notificacaoDto);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Route("{notificacaoId}/marcar-como-lida")]
        public IActionResult MarcarComoLida(long notificacaoId)
        {
            comandosNotificacao.MarcarComoLida(notificacaoId);
            return Ok();
        }
    }
}