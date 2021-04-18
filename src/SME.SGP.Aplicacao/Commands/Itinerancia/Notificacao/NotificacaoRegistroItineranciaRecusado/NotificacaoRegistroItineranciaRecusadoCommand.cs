using MediatR;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoRegistroItineranciaRecusadoCommand : IRequest<bool>
    {
        public NotificacaoRegistroItineranciaRecusadoCommand(long itineranciaId, long workflowId)
        {
            ItineranciaId = itineranciaId;
            WorkflowId = workflowId;
        }

        public long ItineranciaId { get; set; }
        public long WorkflowId { get; set; }
    }
}
