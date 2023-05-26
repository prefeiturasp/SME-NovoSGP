using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterMensagemNotificacaoAlteracaoParecerConclusivoQuery : IRequest<string>
    {
        public ObterMensagemNotificacaoAlteracaoParecerConclusivoQuery(long workflowAprovacaoId, long notificacaoId)
        {
            WorkflowAprovacaoId = workflowAprovacaoId;
            NotificacaoId = notificacaoId;
        }

        public long WorkflowAprovacaoId { get; }
        public long NotificacaoId { get; }
    }
}