using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarWorkflowAprovacaoItineranciaCommandHandler : IRequestHandler<SalvarWorkflowAprovacaoItineranciaCommand, bool>
    {
        private readonly IRepositorioWfAprovacaoItinerancia repositorioWfAprovacaoItinerancia;

        public SalvarWorkflowAprovacaoItineranciaCommandHandler(IRepositorioWfAprovacaoItinerancia repositorioWfAprovacaoItinerancia)
        {
            this.repositorioWfAprovacaoItinerancia = repositorioWfAprovacaoItinerancia ?? throw new ArgumentNullException(nameof(repositorioWfAprovacaoItinerancia));
        }

        public async Task<bool> Handle(SalvarWorkflowAprovacaoItineranciaCommand request, CancellationToken cancellationToken)
        {
            WfAprovacaoItinerancia wfAprovacaoItinerancia = new WfAprovacaoItinerancia
            {
                ItineranciaId = request.ItineranciaId,
                WfAprovacaoId = request.WorkflowId
            };

            await repositorioWfAprovacaoItinerancia.SalvarAsync(wfAprovacaoItinerancia);

            return true;
        }
    }
}
