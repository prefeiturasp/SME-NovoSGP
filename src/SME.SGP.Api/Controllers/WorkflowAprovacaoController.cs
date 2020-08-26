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
    [Route("api/v1/workflows/aprovacoes")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class WorkflowAprovacaoController : ControllerBase
    {
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;
        private readonly IConsultasWorkflowAprovacao consultasWorkflowAprovacao;

        public WorkflowAprovacaoController(IComandosWorkflowAprovacao comandosWorkflowAprovacao, IConsultasWorkflowAprovacao consultasWorkflowAprovacao)
        {
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new System.ArgumentNullException(nameof(comandosWorkflowAprovacao));
            this.consultasWorkflowAprovacao = consultasWorkflowAprovacao ?? throw new System.ArgumentNullException(nameof(consultasWorkflowAprovacao));
        }

        [HttpPut]
        [Route("notificacoes/{notificacaoId}/aprova")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Aprovar(long notificacaoId, [FromBody] WorkflowAprovacaoAprovacaoDto workflowAprovacaoAprovacaoDto)
        {
            await comandosWorkflowAprovacao.Aprovar(workflowAprovacaoAprovacaoDto.Aprova, notificacaoId, workflowAprovacaoAprovacaoDto.Observacao);
            return Ok();
        }

        [HttpGet]
        [Route("notificacoes/{id}/linha-tempo")]
        [ProducesResponseType(typeof(IEnumerable<WorkflowAprovacaoTimeRespostaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult ObterLinhaDoTempo(long id)
        {
            return Ok(consultasWorkflowAprovacao.ObtemTimelinePorCodigoNotificacao(id));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Post(WorkflowAprovacaoDto workflowAprovaNivelDto)
        {
            await comandosWorkflowAprovacao.Salvar(workflowAprovaNivelDto);
            return Ok();
        }
    }
}