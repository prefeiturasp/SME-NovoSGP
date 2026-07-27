using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ClassificacaoDocumentoMap : SimpleEntityMap<ClassificacaoDocumento>
    {
        public ClassificacaoDocumentoMap()
        {
            ToTable("classificacao_documento");
            Map(nameof(ClassificacaoDocumento.TipoDocumentoId), "tipo_documento_id");
            Map(nameof(ClassificacaoDocumento.Descricao), "descricao");
            Map(nameof(ClassificacaoDocumento.EhRegistroMultiplo), "ehregistromultiplo");
        }
    }
}