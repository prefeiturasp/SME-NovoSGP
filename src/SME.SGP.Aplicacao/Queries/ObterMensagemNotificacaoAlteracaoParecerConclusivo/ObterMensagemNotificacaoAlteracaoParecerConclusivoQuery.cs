using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterMensagemNotificacaoAlteracaoNotaPosConselhoQuery : IRequest<string>
    {
        public ObterMensagemNotificacaoAlteracaoNotaPosConselhoQuery(long workflowAprovacaoId, long notificacaoId)
        {
            WorkflowAprovacaoId = workflowAprovacaoId;
            NotificacaoId = notificacaoId;
        }

        public long WorkflowAprovacaoId { get; }
        public long NotificacaoId { get; }
    }
}