using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DocumentoArquivoMap : SimpleMap<DocumentoArquivo>
    {
        public DocumentoArquivoMap()
        {
            ToTable("documento_arquivo");
            Map(nameof(DocumentoArquivo.DocumentoId), "documento_id");
            Map(nameof(DocumentoArquivo.ArquivoId), "arquivo_id");
        }
    }
}