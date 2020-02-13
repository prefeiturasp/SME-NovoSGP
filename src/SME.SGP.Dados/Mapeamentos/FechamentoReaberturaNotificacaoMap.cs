using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class FechamentoReaberturaNotificacaoMap : DommelEntityMap<FechamentoReaberturaNotificacao>
    {
        public FechamentoReaberturaNotificacaoMap()
        {
            ToTable("fechamento_reabertura_notificacao");
            Map(a => a.FechamentoReaberturaId).ToColumn("fechamento_reabertura_id");
            Map(a => a.NotificacaoId).ToColumn("notificacao_id");
        }
    }
}