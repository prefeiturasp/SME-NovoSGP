using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaQuestaoMap : BaseMap<ItineranciaQuestao>
    {
        public ItineranciaQuestaoMap()
        {
            ToTable("itinerancia_questao");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.ItineranciaId).ToColumn("itinerancia_id");
        }
    }
}
