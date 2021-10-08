using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoCalendarioMap : BaseMap<TipoCalendario>
    {
        public TipoCalendarioMap()
        {
            ToTable("tipo_calendario");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.Modalidade).ToColumn("modalidade");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Periodo).ToColumn("periodo");
            Map(c => c.Situacao).ToColumn("situacao");
        }
    }
}
