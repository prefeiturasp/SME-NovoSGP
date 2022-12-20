using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DocumentoArquivoMap : DommelEntityMap<DocumentoArquivo>
    {
        public DocumentoArquivoMap()
        {
            ToTable("documento_arquivo");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.DocumentoId).ToColumn("documento_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
        }
    }
}