using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoDocumentoMap : SimpleEntityMap<TipoDocumento>
    {
        public TipoDocumentoMap()
        {
            ToTable("tipo_documento");

            Map(nameof(TipoDocumento.Descricao), "descricao");
        }
    }
}