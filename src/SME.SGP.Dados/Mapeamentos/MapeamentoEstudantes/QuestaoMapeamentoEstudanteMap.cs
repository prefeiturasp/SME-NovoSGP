using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class QuestaoMapeamentoEstudanteMap : BaseEntityMap<QuestaoMapeamentoEstudante>
    {
        public QuestaoMapeamentoEstudanteMap()
        {
            ToTable("mapeamento_estudante_questao");

            Map(nameof(QuestaoMapeamentoEstudante.MapeamentoEstudanteSecaoId), "mapeamento_estudante_secao_id");
            Map(nameof(QuestaoMapeamentoEstudante.QuestaoId), "questao_id");
            Map(nameof(QuestaoMapeamentoEstudante.Excluido), "excluido");
        }
    }
}