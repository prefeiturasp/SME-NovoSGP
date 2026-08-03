using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class FechamentoReaberturaNotificacaoMap : SimpleMap<FechamentoReaberturaNotificacao>
    {
        public FechamentoReaberturaNotificacaoMap()
        {
            ToTable("fechamento_reabertura_notificacao");
            Map(nameof(FechamentoReaberturaNotificacao.FechamentoReaberturaId), "fechamento_reabertura_id");
            Map(nameof(FechamentoReaberturaNotificacao.NotificacaoId), "notificacao_id");
        }
    }
}