using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ItineranciaEventoMap : BaseEntityMap<ItineranciaEvento>
    {
        public ItineranciaEventoMap()
        {
            ToTable("itinerancia_evento");
            Map(nameof(ItineranciaEvento.ItineranciaId), "itinerancia_id");
            Map(nameof(ItineranciaEvento.EventoId), "evento_id");
            Map(nameof(ItineranciaEvento.Excluido), "excluido");
        }
    }
}