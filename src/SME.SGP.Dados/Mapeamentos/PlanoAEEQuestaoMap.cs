using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoAEEQuestaoMap : BaseMap<PlanoAEEQuestao>
    {
        public PlanoAEEQuestaoMap()
        {
            ToTable("plano_aee_questao");
            Map(nameof(PlanoAEEQuestao.PlanoAEEVersaoId), "plano_aee_versao_id");
            Map(nameof(PlanoAEEQuestao.QuestaoId), "questao_id");
            Map(nameof(PlanoAEEQuestao.Excluido), "excluido");
        }
    }
}