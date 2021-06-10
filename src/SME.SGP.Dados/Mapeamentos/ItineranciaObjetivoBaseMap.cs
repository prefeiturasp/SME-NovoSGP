using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaObjetivoBaseMap : DommelEntityMap<ItineranciaObjetivoBase>
    {
        public ItineranciaObjetivoBaseMap()
        {
            ToTable("itinerancia_objetivo_base");
            Map(a => a.TemDescricao).ToColumn("tem_descricao");
        }
    }
}
