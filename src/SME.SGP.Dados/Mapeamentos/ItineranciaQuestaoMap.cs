using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaQuestaoMap : BaseMap<ItineranciaQuestao>
    {
        public ItineranciaQuestaoMap()
        {
            ToTable("itinerancia_questao");
            Map(nameof(ItineranciaQuestao.QuestaoId), "questao_id");
            Map(nameof(ItineranciaQuestao.ArquivoId), "arquivo_id");
            Map(nameof(ItineranciaQuestao.Resposta), "resposta");
            Map(nameof(ItineranciaQuestao.Excluido), "excluido");
            Map(nameof(ItineranciaQuestao.ItineranciaId), "itinerancia_id");
        }
    }
}