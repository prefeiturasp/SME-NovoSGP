using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowItineranciaPorItineranciaIdQuery : IRequest<WfAprovacaoItinerancia>
    {
        public ObterWorkflowItineranciaPorItineranciaIdQuery(long itineranciaId)
        {
            ItineranciaId = itineranciaId;
        }
        public long ItineranciaId { get; set; }
    }
}
