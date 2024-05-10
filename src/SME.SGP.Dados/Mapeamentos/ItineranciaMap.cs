using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaMap : BaseMap<Itinerancia>
    {
        public ItineranciaMap()
        {
            ToTable("itinerancia");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.EventoId).ToColumn("evento_id");
            Map(c => c.DataVisita).ToColumn("data_visita");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.DataRetornoVerificacao).ToColumn("data_retorno_verificacao");
            Map(c => c.Situacao).ToColumn("situacao");
        }
    }
}
