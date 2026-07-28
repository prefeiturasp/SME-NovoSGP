using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class WorkflowAprovacaoNivelNotificacao
    {
        [Key]
        public long Id { get; set; }
        public long NotificacaoId { get; set; }

        public long WorkflowAprovacaoNivelId { get; set; }
    }
}