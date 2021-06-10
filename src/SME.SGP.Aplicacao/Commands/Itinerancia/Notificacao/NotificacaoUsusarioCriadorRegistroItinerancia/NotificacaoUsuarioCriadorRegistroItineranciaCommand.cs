using MediatR;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoUsuarioCriadorRegistroItineranciaCommand : IRequest<bool>
    {
        public NotificacaoUsuarioCriadorRegistroItineranciaCommand(long itineranciaId, long workflowId)
        {
            ItineranciaId = itineranciaId;
            WorkflowId = workflowId;
        }

        public long ItineranciaId { get; set; }
        public long WorkflowId { get; set; }

    }
}
