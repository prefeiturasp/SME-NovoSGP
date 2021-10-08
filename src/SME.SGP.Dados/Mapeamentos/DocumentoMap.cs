using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DocumentoMap : BaseMap<Documento>
    {
        public DocumentoMap()
        {
            ToTable("documento");
            Map(c => c.ClassificacaoDocumentoId).ToColumn("classificacao_documento_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
        }
    }
}
