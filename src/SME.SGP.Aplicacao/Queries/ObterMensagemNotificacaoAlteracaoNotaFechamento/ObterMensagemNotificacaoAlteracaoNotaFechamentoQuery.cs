using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterMensagemNotificacaoAlteracaoNotaFechamentoQuery : IRequest<string>
    {
        public ObterMensagemNotificacaoAlteracaoNotaFechamentoQuery(long workflowAprovacaoId, long notificacaoId)
        {
            WorkflowAprovacaoId = workflowAprovacaoId;
            NotificacaoId = notificacaoId;
        }

        public long WorkflowAprovacaoId { get; }
        public long NotificacaoId { get; }
    }
}