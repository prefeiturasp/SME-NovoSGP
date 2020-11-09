using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DocumentoMap : BaseMap<Documento>
    {
        public DocumentoMap()
        {
            ToTable("documento");
            Map(a => a.ClassificacaoDocumentoId).ToColumn("classificacao_documento_id");
            Map(a => a.UsuarioRf).ToColumn("usuario_rf");
            Map(a => a.ArquivoId).ToColumn("arquivo_id");
        }
    }
}
