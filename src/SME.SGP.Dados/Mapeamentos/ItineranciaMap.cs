using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaMap : BaseMap<Itinerancia>
    {
        public ItineranciaMap()
        {
            ToTable("itinerancia");
            Map(c => c.DataVisita).ToColumn("data_visita");
            Map(c => c.DataRetornoVerificacao).ToColumn("data_retorno_verificacao");
            Map(c => c.Situacao).ToColumn("situacao");
        }
    }
}
