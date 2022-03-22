using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ItineranciaEventoMap : BaseMap<ItineranciaEvento>
    {
        public ItineranciaEventoMap()
        {
            ToTable("itinerancia_evento");
            Map(c => c.ItineranciaId).ToColumn("itinerancia_id");
            Map(c => c.EventoId).ToColumn("evento_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
