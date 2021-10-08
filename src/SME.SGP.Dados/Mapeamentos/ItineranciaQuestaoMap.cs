using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaQuestaoMap : BaseMap<ItineranciaQuestao>
    {
        public ItineranciaQuestaoMap()
        {
            ToTable("itinerancia_questao");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Resposta).ToColumn("resposta");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.ItineranciaId).ToColumn("itinerancia_id");
        }
    }
}
