using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/aula-prevista")]
    [ValidaDto]
    public class AulaPrevistaController : ControllerBase
    {
        [HttpGet("modalidades/{modalidade}/turmas/{turmaId}/disciplinas/{disciplinaId}")]
        [ProducesResponseType(typeof(AulasPrevistasDadasAuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.ADAP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAulaPrevistaDada(Modalidade modalidade, string turmaId, string disciplinaId, [FromServices]IConsultasAulaPrevista consultas)
        {
            return Ok(await consultas.ObterAulaPrevistaDada(modalidade, turmaId, disciplinaId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ADAP_I, Policy = "Bearer")]
        public async Task<IActionResult> Inserir([FromBody]AulaPrevistaDto dto, [FromServices]IComandosAulaPrevista comandos)
        {
            await comandos.Inserir(dto);
            return Ok();
        }

        [HttpPost("notificar")]
        public async Task<IActionResult> Notificar([FromServices] IServicoNotificacaoAulaPrevista servicoNotificacaoAulaPrevista)
        {
            servicoNotificacaoAulaPrevista.ExecutaNotificacaoAulaPrevista();
            return Ok();
        }
    }
}
