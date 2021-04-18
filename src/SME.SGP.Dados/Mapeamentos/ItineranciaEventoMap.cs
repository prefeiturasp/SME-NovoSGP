using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ItineranciaEventoMap : BaseMap<ItineranciaEvento>
    {
        public ItineranciaEventoMap()
        {
            ToTable("itinerancia_evento");
            Map(a => a.ItineranciaId).ToColumn("itinerancia_id");
            Map(a => a.EventoId).ToColumn("evento_id");
        }
    }
}
