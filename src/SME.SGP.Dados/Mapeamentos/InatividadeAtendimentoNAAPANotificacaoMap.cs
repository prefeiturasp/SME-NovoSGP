using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InatividadeAtendimentoNAAPANotificacaoMap : BaseMap<InatividadeAtendimentoNAAPANotificacao>
    {
        public InatividadeAtendimentoNAAPANotificacaoMap()
        {
            ToTable("inatividade_atendimento_naapa_notificacao");
            Map(c => c.EncaminhamentoNAAPAId).ToColumn("encaminhamento_naapa_id");
            Map(c => c.NotificacaoId).ToColumn("notificacao_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
