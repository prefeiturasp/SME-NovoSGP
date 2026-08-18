using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaMap : BaseMap<Itinerancia>
    {
        public ItineranciaMap()
        {
            ToTable("itinerancia");
            Map(nameof(Itinerancia.DreId), "dre_id");
            Map(nameof(Itinerancia.UeId), "ue_id");
            Map(nameof(Itinerancia.EventoId), "evento_id");
            Map(nameof(Itinerancia.DataVisita), "data_visita");
            Map(nameof(Itinerancia.AnoLetivo), "ano_letivo");
            Map(nameof(Itinerancia.DataRetornoVerificacao), "data_retorno_verificacao");
            Map(nameof(Itinerancia.Situacao), "situacao");
        }
    }
}