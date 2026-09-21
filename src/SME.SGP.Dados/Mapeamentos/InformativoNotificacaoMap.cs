using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InformativoNotificacaoMap : BaseMap<InformativoNotificacao>
    {
        public InformativoNotificacaoMap()
        {
            ToTable("informativo_notificacao");
            Map(nameof(InformativoNotificacao.InformativoId), "informativo_id");
            Map(nameof(InformativoNotificacao.NotificacaoId), "notificacao_id");
            Map(nameof(InformativoNotificacao.Excluido), "excluido");
        }
    }
}