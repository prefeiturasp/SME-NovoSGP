using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ClassificacaoDocumentoMap : DommelEntityMap<ClassificacaoDocumento>
    {
        public ClassificacaoDocumentoMap()
        {
            ToTable("classificacao_documento");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.TipoDocumentoId).ToColumn("tipo_documento_id");
            Map(c => c.Descricao).ToColumn("descricao");
        }
    }
}