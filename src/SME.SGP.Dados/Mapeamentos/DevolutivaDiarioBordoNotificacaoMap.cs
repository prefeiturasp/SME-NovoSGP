using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DevolutivaDiarioBordoNotificacaoMap : SimpleEntityMap<NotificacaoDevolutiva>
    {
        public DevolutivaDiarioBordoNotificacaoMap()
        {
            ToTable("devolutiva_diario_bordo_notificacao");
            Map(nameof(NotificacaoDevolutiva.DevolutivaId), "devolutiva_id");
            Map(nameof(NotificacaoDevolutiva.NotificacaoId), "notificacao_id");
        }
    }
}