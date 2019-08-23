using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class BaseMap<T> : DommelEntityMap<T> where T : EntidadeBase
    {
        public BaseMap()
        {
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.CriadoPor).ToColumn("criado_por");
            Map(c => c.AlteradoEm).ToColumn("alterado_em");
            Map(c => c.AlteradoPor).ToColumn("alterado_por");
            Map(c => c.AlteradoRF).ToColumn("alterado_rf");
            Map(c => c.CriadoRF).ToColumn("criado_rf");
        }
    }
}