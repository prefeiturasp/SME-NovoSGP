using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoCompensacaoAusenciaMap : SimpleEntityMap<NotificacaoCompensacaoAusencia>
    {
        public NotificacaoCompensacaoAusenciaMap()
        {
            ToTable("notificacao_compensacao_ausencia");
            Map(nameof(NotificacaoCompensacaoAusencia.NotificacaoId), "notificacao_id");
            Map(nameof(NotificacaoCompensacaoAusencia.CompensacaoAusenciaId), "compensacao_ausencia_id");
        }
    }
}