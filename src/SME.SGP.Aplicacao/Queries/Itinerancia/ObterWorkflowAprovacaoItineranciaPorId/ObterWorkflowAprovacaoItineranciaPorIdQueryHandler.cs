using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowAprovacaoItineranciaPorIdQueryHandler : IRequestHandler<ObterWorkflowAprovacaoItineranciaPorIdQuery, WfAprovacaoItinerancia>
    {
        private readonly IRepositorioWfAprovacaoItinerancia repositorioWfAprovacaoItinerancia;

        public ObterWorkflowAprovacaoItineranciaPorIdQueryHandler(IRepositorioWfAprovacaoItinerancia repositorioWfAprovacaoItinerancia)
        {
            this.repositorioWfAprovacaoItinerancia = repositorioWfAprovacaoItinerancia ?? throw new ArgumentNullException(nameof(repositorioWfAprovacaoItinerancia));
        }

        public async Task<WfAprovacaoItinerancia> Handle(ObterWorkflowAprovacaoItineranciaPorIdQuery request, CancellationToken cancellationToken)
                       => await repositorioWfAprovacaoItinerancia.ObterPorWorkflowId(request.WorkflowId);
    }
}
