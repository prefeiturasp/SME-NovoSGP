using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class UEMap : DommelEntityMap<UE>
    {
        public UEMap()
        {
            ToTable("ue");
            Map(c => c.UEId).ToColumn("ue_id");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.Tipo).ToColumn("tipo_escola");
        }
    }
}