using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DocumentoMap : BaseMap<Documento>
    {
        public DocumentoMap()
        {
            ToTable("documento");
            Map(a => a.ClassificacaoDocumentoId).ToColumn("classificacao_documento_id");
            Map(a => a.UsuarioId).ToColumn("usuario_id");
            Map(a => a.AnoLetivo).ToColumn("ano_letivo");
            Map(a => a.UeId).ToColumn("ue_id");
            Map(a => a.ArquivoId).ToColumn("arquivo_id");
        }
    }
}
