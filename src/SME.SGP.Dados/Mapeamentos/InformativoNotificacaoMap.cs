using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InformativoNotificacaoMap : BaseMap<InformativoNotificacao>
    {
        public InformativoNotificacaoMap()
        {
            ToTable("informativo_notificacao");
            Map(c => c.InformativoId).ToColumn("informativo_id");
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
