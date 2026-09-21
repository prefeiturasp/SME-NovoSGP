using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoAulaMap : SimpleMap<NotificacaoAula>
    {
        public NotificacaoAulaMap()
        {
            ToTable("notificacao_aula");
            Map(nameof(NotificacaoAula.NotificacaoId), "notificacao_id");
            Map(nameof(NotificacaoAula.AulaId), "aula_id");
        }
    }
}