using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoCalendarioEscolarMap : BaseMap<TipoCalendarioEscolar>
    {
        public TipoCalendarioEscolarMap()
        {
            ToTable("tipo_calendario_escolar");
            Map(t => t.AnoLetivo).ToColumn("ano_letivo");
            Map(t => t.Nome).ToColumn("nome");
            Map(t => t.Periodo).ToColumn("periodo");
            Map(t => t.Situacao).ToColumn("situacao");
        }
    }
}
