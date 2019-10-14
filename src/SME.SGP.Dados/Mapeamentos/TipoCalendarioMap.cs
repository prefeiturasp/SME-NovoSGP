using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoCalendarioMap : BaseMap<TipoCalendario>
    {
        public TipoCalendarioMap()
        {
            ToTable("tipo_calendario");
            Map(t => t.AnoLetivo).ToColumn("ano_letivo");
            Map(t => t.Nome).ToColumn("nome");
            Map(t => t.Periodo).ToColumn("periodo");
            Map(t => t.Situacao).ToColumn("situacao");
            Map(t => t.Modalidade).ToColumn("modalidade");
        }
    }
}
