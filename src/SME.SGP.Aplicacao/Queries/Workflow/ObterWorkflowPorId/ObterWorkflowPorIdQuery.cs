using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowPorIdQuery : IRequest<WorkflowAprovacao>
    {
        public ObterWorkflowPorIdQuery(long workflowId)
        {
            WorkflowId = workflowId;
        }
        public long WorkflowId { get; set; }
    }
}
