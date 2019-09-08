using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IComandosWorkflowAprovaNivel
    {
        void SalvarListaDto(IEnumerable<WorkflowAprovaNivelDto> workflowAprovacaoNiveisDto);
    }
}