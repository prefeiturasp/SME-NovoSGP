using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowPorIdQueryHandler : IRequestHandler<ObterWorkflowPorIdQuery, WorkflowAprovacao>
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;

        public ObterWorkflowPorIdQueryHandler(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
        }

        public async Task<WorkflowAprovacao> Handle(ObterWorkflowPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioWorkflowAprovacao.ObterEntidadeCompletaPorId(request.WorkflowId);
        }
    }
}
