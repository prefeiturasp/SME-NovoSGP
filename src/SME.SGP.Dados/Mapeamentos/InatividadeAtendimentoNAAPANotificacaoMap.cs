using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InatividadeAtendimentoNAAPANotificacaoMap : BaseEntityMap<InatividadeAtendimentoNAAPANotificacao>
    {
        public InatividadeAtendimentoNAAPANotificacaoMap()
        {
            ToTable("inatividade_atendimento_naapa_notificacao");
            Map(nameof(InatividadeAtendimentoNAAPANotificacao.EncaminhamentoNAAPAId), "encaminhamento_naapa_id");
            Map(nameof(InatividadeAtendimentoNAAPANotificacao.NotificacaoId), "notificacao_id");
            Map(nameof(InatividadeAtendimentoNAAPANotificacao.Excluido), "excluido");
        }
    }
}