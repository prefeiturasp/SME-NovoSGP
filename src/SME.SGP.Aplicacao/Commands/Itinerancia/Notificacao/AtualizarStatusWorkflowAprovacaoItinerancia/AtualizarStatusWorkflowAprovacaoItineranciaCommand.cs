using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AtualizarStatusWorkflowAprovacaoItineranciaCommand : IRequest<bool>
    {
        public AtualizarStatusWorkflowAprovacaoItineranciaCommand(long itineranciaId, long workflowId, bool statusAprovacao)
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
