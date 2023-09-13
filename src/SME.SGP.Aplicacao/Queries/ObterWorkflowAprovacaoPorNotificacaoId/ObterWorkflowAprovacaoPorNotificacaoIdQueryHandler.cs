using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowAprovacaoPorNotificacaoIdQueryHandler : IRequestHandler<ObterWorkflowAprovacaoPorNotificacaoIdQuery, WorkflowAprovacao>
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;

        public ObterWorkflowAprovacaoPorNotificacaoIdQueryHandler(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
        }

        public async Task<WorkflowAprovacao> Handle(ObterWorkflowAprovacaoPorNotificacaoIdQuery request, CancellationToken cancellationToken)
        {
            var workflow = await repositorioWorkflowAprovacao.ObterEntidadeCompleta(0, request.NotificacaoId);
            
            if (workflow.EhNulo())
                throw new NegocioException($"Não foi possível localizar o fluxo de aprovação da notificação {request.NotificacaoId}");

            return workflow;
        }
    }
}