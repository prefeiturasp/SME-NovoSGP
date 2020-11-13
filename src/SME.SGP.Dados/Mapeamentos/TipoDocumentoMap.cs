using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoDocumentoMap : DommelEntityMap<TipoDocumento>
    {
        public TipoDocumentoMap()
        {
            ToTable("tipo_documento");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.Descricao).ToColumn("descricao");
        }
    }
}