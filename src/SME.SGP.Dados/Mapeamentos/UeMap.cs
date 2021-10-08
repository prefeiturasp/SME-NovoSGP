using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class UeMap : DommelEntityMap<Ue>
    {
        public UeMap()
        {
            ToTable("ue");
            Map(c => c.CodigoUe).ToColumn("ue_id");
            Map(c => c.DataAtualizacao).ToColumn("data_atualizacao");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.TipoEscola).ToColumn("tipo_escola");
        }
    }
}