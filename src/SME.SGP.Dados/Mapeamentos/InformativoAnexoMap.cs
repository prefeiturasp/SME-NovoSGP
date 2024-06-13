using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InformativoAnexoMap : DommelEntityMap<InformativoAnexo>
    {
        public InformativoAnexoMap()
        {
            ToTable("informativo_anexo");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.InformativoId).ToColumn("informativo_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
        }
    }
}