using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaObjetivoMap : BaseMap<ItineranciaObjetivo>
    {
        public ItineranciaObjetivoMap()
        {
            ToTable("itinerancia_objetivo");
            Map(c => c.ItineranciaObjetivosBaseId).ToColumn("itinerancia_base_id");
            Map(c => c.ItineranciaId).ToColumn("itinerancia_id");
        }
    }
}
