using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class WorkflowAprovacaoNivelNotificacao
    {
        public long Id { get; set; }
        public long NotificacaoId { get; set; }

        public long WorkflowAprovacaoNivelId { get; set; }
    }
}