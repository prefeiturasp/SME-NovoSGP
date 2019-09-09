using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/workflows/aprova-niveis")]
    [ValidaDto]
    public class WorkflowAprovaNivelController : ControllerBase
    {
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;

        public WorkflowAprovaNivelController(IComandosWorkflowAprovacao comandosWorkflowAprovacao)
        {
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new System.ArgumentNullException(nameof(comandosWorkflowAprovacao));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Post(WorkflowAprovacaoNiveisDto workflowAprovaNivelDto)
        {
            comandosWorkflowAprovacao.SalvarListaDto(workflowAprovaNivelDto);
            return Ok();
        }
    }
}