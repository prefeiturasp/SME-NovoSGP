using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaObjetivoMap : BaseMap<ItineranciaObjetivo>
    {
        public ItineranciaObjetivoMap()
        {
            ToTable("itinerancia_objetivo");
            Map(nameof(ItineranciaObjetivo.ItineranciaObjetivosBaseId), "itinerancia_base_id");
            Map(nameof(ItineranciaObjetivo.ItineranciaId), "itinerancia_id");
            Map(nameof(ItineranciaObjetivo.Descricao), "descricao");
            Map(nameof(ItineranciaObjetivo.Excluido), "excluido");
        }
    }
}