using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarStatusWorkflowAprovacaoItineranciaCommandHandler : IRequestHandler<AtualizarStatusWorkflowAprovacaoItineranciaCommand, bool>
    {
        private readonly IRepositorioWfAprovacaoItinerancia repositorioWfAprovacaoItinerancia;

        public AtualizarStatusWorkflowAprovacaoItineranciaCommandHandler(IRepositorioWfAprovacaoItinerancia repositorioWfAprovacaoItinerancia)
        {
            this.repositorioWfAprovacaoItinerancia = repositorioWfAprovacaoItinerancia ?? throw new ArgumentNullException(nameof(repositorioWfAprovacaoItinerancia));
        }

        public async Task<bool> Handle(AtualizarStatusWorkflowAprovacaoItineranciaCommand request, CancellationToken cancellationToken)
        {

            var wfAprovacaoItinerancia = await repositorioWfAprovacaoItinerancia.ObterPorWorkflowId(request.WorkflowId);

            await repositorioWfAprovacaoItinerancia.SalvarAsync(wfAprovacaoItinerancia);

            return true;
        }
    }
}
