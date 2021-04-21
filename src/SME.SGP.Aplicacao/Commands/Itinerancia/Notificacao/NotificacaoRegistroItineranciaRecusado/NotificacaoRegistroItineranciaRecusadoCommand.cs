using MediatR;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoRegistroItineranciaRecusadoCommand : IRequest<bool>
    {
        public NotificacaoRegistroItineranciaRecusadoCommand(long itineranciaId, long workflowId, string observacoes)
        {
            ItineranciaId = itineranciaId;
            WorkflowId = workflowId;
            Observacoes = observacoes;
        }

        public long ItineranciaId { get; set; }
        public long WorkflowId { get; set; }
        public string Observacoes { get; set; }
    }
}
