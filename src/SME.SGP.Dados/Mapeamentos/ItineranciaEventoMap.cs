using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class ItineranciaEventoMap : BaseMap<ItineranciaEvento>
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