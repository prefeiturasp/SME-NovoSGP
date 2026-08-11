using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class QuestaoEncaminhamentoAEEMap : BaseMap<QuestaoEncaminhamentoAEE>
    {
        public QuestaoEncaminhamentoAEEMap()
        {
            ToTable("questao_encaminhamento_aee");
            Map(nameof(QuestaoEncaminhamentoAEE.EncaminhamentoAEESecaoId), "encaminhamento_aee_secao_id");
            Map(nameof(QuestaoEncaminhamentoAEE.QuestaoId), "questao_id");
            Map(nameof(QuestaoEncaminhamentoAEE.Excluido), "excluido");
        }
    }
}