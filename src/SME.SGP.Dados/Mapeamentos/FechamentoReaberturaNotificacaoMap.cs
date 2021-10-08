using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class FechamentoReaberturaNotificacaoMap : DommelEntityMap<FechamentoReaberturaNotificacao>
    {
        public FechamentoReaberturaNotificacaoMap()
        {
            ToTable("fechamento_reabertura_notificacao");
            Map(c => c.FechamentoReaberturaId).ToColumn("fechamento_reabertura_id");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
        }
    }
}