using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowAprovacaoPorNotificacaoIdQuery : IRequest<WorkflowAprovacao>
    {
        public ObterWorkflowAprovacaoPorNotificacaoIdQuery(long notificacaoId)
        {
            NotificacaoId = notificacaoId;
        }

        public long NotificacaoId { get; }
    }
}