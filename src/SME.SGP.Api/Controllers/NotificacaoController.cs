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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Post([FromBody]NotificacaoDto notificacaoDto)
        {
            comandosNotificacao.Salvar(notificacaoDto);
            return Ok();
        }
    }
}