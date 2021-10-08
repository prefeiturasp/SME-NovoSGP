using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaObjetivoBaseMap : DommelEntityMap<ItineranciaObjetivoBase>
    {
        public ItineranciaObjetivoBaseMap()
        {
            ToTable("itinerancia_objetivo_base");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.TemDescricao).ToColumn("tem_descricao");
            Map(c => c.Ordem).ToColumn("ordem");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
