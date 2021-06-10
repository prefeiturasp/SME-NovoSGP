using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowItineranciaPorItineranciaIdQueryHandler : IRequestHandler<ObterWorkflowItineranciaPorItineranciaIdQuery, WfAprovacaoItinerancia>
    {
        private readonly IRepositorioWfAprovacaoItinerancia repositorio;

        public ObterWorkflowItineranciaPorItineranciaIdQueryHandler(IRepositorioWfAprovacaoItinerancia repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<WfAprovacaoItinerancia> Handle(ObterWorkflowItineranciaPorItineranciaIdQuery request, CancellationToken cancellationToken)
                       => await repositorio.ObterPorItineranciaId(request.ItineranciaId);
    }
}
