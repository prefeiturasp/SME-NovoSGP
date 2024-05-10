using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CicloAnoMap : DommelEntityMap<CicloAno>
    {
        public CicloAnoMap()
        {
            ToTable("tipo_ciclo_ano");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.CicloId).ToColumn("tipo_ciclo_id");
            Map(c => c.Modalidade).ToColumn("modalidade");
            Map(c => c.Ano).ToColumn("ano");
        }
    }
}
