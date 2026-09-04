using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class DocumentoMap : BaseMap<Documento>
    {
        public DocumentoMap()
        {
            ToTable("documento");
            Map(nameof(Documento.ClassificacaoDocumentoId), "classificacao_documento_id");
            Map(nameof(Documento.UsuarioId), "usuario_id");
            Map(nameof(Documento.UeId), "ue_id");
            Map(nameof(Documento.AnoLetivo), "ano_letivo");
            Map(nameof(Documento.TurmaId), "turma_id");
            Map(nameof(Documento.ComponenteCurricularId), "componente_curricular_id");
        }
    }
}