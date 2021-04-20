using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowAprovacaoItineranciaPorIdQuery : IRequest<WfAprovacaoItinerancia>
    {
        public ObterWorkflowAprovacaoItineranciaPorIdQuery(long workflowId)
        {
            WorkflowId = workflowId;
        }
        public long WorkflowId { get; set; }
    }
}
