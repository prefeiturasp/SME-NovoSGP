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
        private readonly IMediator mediator;

        public SalvarWorkflowAprovacaoItineranciaCommandHandler(IRepositorioWfAprovacaoItinerancia repositorioWfAprovacaoItinerancia, IMediator mediator)
        {
            this.repositorioWfAprovacaoItinerancia = repositorioWfAprovacaoItinerancia ?? throw new ArgumentNullException(nameof(repositorioWfAprovacaoItinerancia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(SalvarWorkflowAprovacaoItineranciaCommand request, CancellationToken cancellationToken)
        {
            WfAprovacaoItinerancia wfAprovacaoItinerancia = new WfAprovacaoItinerancia
            {
                ItineranciaId = request.ItineranciaId,
                WfAprovacaoId = request.WorkflowId
            };

            await repositorioWfAprovacaoItinerancia.SalvarAsync(wfAprovacaoItinerancia);
            await mediator.Send(new AlterarSituacaoItineranciaCommand(request.ItineranciaId, Dominio.Enumerados.SituacaoItinerancia.Enviado));

            return true;
        }
    }
}
