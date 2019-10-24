using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FeriadoCalendarioMap : BaseMap<FeriadoCalendario>
    {
        public FeriadoCalendarioMap()
        {
            ToTable("feriado_calendario");
            Map(t => t.Abrangencia).ToColumn("abrangencia");
            Map(t => t.Nome).ToColumn("nome");
            Map(t => t.DataFeriado).ToColumn("data_feriado");
            Map(t => t.Ativo).ToColumn("ativo");
            Map(t => t.Tipo).ToColumn("tipo");
        }
    }
}