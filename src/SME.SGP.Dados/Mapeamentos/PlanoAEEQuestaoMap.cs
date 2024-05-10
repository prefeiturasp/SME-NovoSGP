using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoAEEQuestaoMap : BaseMap<PlanoAEEQuestao>
    {
        public PlanoAEEQuestaoMap()
        {
            ToTable("plano_aee_questao");
            Map(c => c.PlanoAEEVersaoId).ToColumn("plano_aee_versao_id");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
