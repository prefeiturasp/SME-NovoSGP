using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarWorkflowAprovacaoItineranciaCommand : IRequest<bool>
    {
        public SalvarWorkflowAprovacaoItineranciaCommand(long itineranciaId, long workflowId)
        {
            ItineranciaId = itineranciaId;
            WorkflowId = workflowId;
        }

        public long ItineranciaId { get; set; }
        public long WorkflowId { get; set; }
    }
}
