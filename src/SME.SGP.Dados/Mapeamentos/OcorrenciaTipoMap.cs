using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaTipoMap : BaseMap<OcorrenciaTipo>
    {
        public OcorrenciaTipoMap()
        {
            ToTable("ocorrencia_tipo");
            Map(x => x.Descricao).ToColumn("descricao");
        }
    }
}