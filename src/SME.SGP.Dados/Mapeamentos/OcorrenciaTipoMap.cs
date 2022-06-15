using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaTipoMap : BaseMap<OcorrenciaTipo>
    {
        public OcorrenciaTipoMap()
        {
            ToTable("ocorrencia_tipo");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}