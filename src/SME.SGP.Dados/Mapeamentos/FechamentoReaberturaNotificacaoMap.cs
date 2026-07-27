using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class FechamentoReaberturaNotificacaoMap : SimpleEntityMap<FechamentoReaberturaNotificacao>
    {
        public FechamentoReaberturaNotificacaoMap()
        {
            ToTable("fechamento_reabertura_notificacao");
            Map(nameof(FechamentoReaberturaNotificacao.FechamentoReaberturaId), "fechamento_reabertura_id");
            Map(nameof(FechamentoReaberturaNotificacao.NotificacaoId), "notificacao_id");
        }
    }
}