using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaUeMap : BaseMap<ItineranciaUe>
    {
        public ItineranciaUeMap()
        {
            ToTable("itinerancia_ue");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.ItineranciaId).ToColumn("itinerancia_id");
        }
    }
}
