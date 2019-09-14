using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/workflows/aprovacoes")]
    [ValidaDto]
    public class WorkflowAprovacaoController : ControllerBase
    {
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;

        public WorkflowAprovacaoController(IComandosWorkflowAprovacao comandosWorkflowAprovacao)
        {
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new System.ArgumentNullException(nameof(comandosWorkflowAprovacao));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Post(WorkflowAprovacaoDto workflowAprovaNivelDto)
        {
            comandosWorkflowAprovacao.Salvar(workflowAprovaNivelDto);
            return Ok();
        }
    }
}