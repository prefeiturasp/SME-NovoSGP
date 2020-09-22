using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComponenteCurricularMap : BaseMap<ComponenteCurricularJurema>
    {
        public ComponenteCurricularMap()
        {
            ToTable("componente_curricular_jurema");
            Map(c => c.DescricaoEOL).ToColumn("descricao_eol");
            Map(c => c.CodigoEOL).ToColumn("codigo_eol");
            Map(c => c.CodigoJurema).ToColumn("codigo_jurema");
        }
    }
}