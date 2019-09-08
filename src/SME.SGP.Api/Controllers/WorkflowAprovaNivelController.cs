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
        private readonly IComandosWorkflowAprovaNivel comandosWorkflowAprovaNivel;

        public WorkflowAprovaNivelController(IComandosWorkflowAprovaNivel comandosWorkflowAprovaNivel)
        {
            this.comandosWorkflowAprovaNivel = comandosWorkflowAprovaNivel ?? throw new System.ArgumentNullException(nameof(comandosWorkflowAprovaNivel));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Post(WorkflowAprovacaoNiveisDto[] workflowAprovaNivelDto)
        {
            comandosWorkflowAprovaNivel.SalvarListaDto(workflowAprovaNivelDto);
            return Ok();
        }
    }
}