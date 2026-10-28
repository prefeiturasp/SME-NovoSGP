using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class QuestaoEncaminhamentoNAAPAMap : BaseMap<QuestaoEncaminhamentoNAAPA>
    {
        public QuestaoEncaminhamentoNAAPAMap()
        {
            ToTable("encaminhamento_naapa_questao");
            Map(nameof(QuestaoEncaminhamentoNAAPA.EncaminhamentoNAAPASecaoId), "encaminhamento_naapa_secao_id");
            Map(nameof(QuestaoEncaminhamentoNAAPA.QuestaoId), "questao_id");
            Map(nameof(QuestaoEncaminhamentoNAAPA.Excluido), "excluido");
        }
    }
}