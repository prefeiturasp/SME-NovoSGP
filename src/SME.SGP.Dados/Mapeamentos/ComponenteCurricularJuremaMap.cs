using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComponenteCurricularJuremaMap : BaseMap<ComponenteCurricularJurema>
    {
        public ComponenteCurricularJuremaMap()
        {
            ToTable("componente_curricular_jurema");
            Map(nameof(ComponenteCurricularJurema.CodigoEOL), "codigo_eol");
            Map(nameof(ComponenteCurricularJurema.CodigoJurema), "codigo_jurema");
            Map(nameof(ComponenteCurricularJurema.DescricaoEOL), "descricao_eol");
        }
    }
}