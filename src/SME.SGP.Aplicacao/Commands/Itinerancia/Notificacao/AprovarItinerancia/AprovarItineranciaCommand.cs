using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AprovarItineranciaCommand : IRequest<bool>
    {
        public AprovarItineranciaCommand(long itineranciaId, long workflowId, bool statusAprovacao)
        {
            ItineranciaId = itineranciaId;
            WorkflowId = workflowId;
            StatusAprovacao = statusAprovacao;
        }

        public long ItineranciaId { get; set; }
        public long WorkflowId { get; set; }
        public bool StatusAprovacao { get; set; }
    }
}
