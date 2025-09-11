using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class NotificacaoPlanoAEEObservacao : EntidadeBase
    {
        public NotificacaoPlanoAEEObservacao() { }
        public NotificacaoPlanoAEEObservacao(long notificacaoId, long planoAEEObservacaoId)
        {
            NotificacaoId = notificacaoId;
            PlanoAEEObservacaoId = planoAEEObservacaoId;
        }

        public PlanoAEEObservacao PlanoAEEObservacao { get; set; }
        public long PlanoAEEObservacaoId { get; set; }

        public Notificacao Notificacao { get; set; }
        public long NotificacaoId { get; set; }

        public bool Excluido { get; set; }
    }
}
