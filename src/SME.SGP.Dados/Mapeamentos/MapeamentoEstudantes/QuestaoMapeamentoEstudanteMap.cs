using SME.SGP.Dominio;
namespace SME.SGP.Dados
{
    public class QuestaoMapeamentoEstudanteMap : BaseMap<QuestaoMapeamentoEstudante>
    {
        public QuestaoMapeamentoEstudanteMap()
        {
            ToTable("mapeamento_estudante_questao");
            Map(c => c.MapeamentoEstudanteSecaoId).ToColumn("mapeamento_estudante_secao_id");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
