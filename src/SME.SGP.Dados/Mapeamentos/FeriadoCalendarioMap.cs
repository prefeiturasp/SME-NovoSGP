using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FeriadoCalendarioMap : BaseMap<FeriadoCalendario>
    {
        public FeriadoCalendarioMap()
        {
            ToTable("feriado_calendario");
            Map(c => c.Abrangencia).ToColumn("abrangencia");
            Map(c => c.Ativo).ToColumn("ativo");
            Map(c => c.DataFeriado).ToColumn("data_feriado");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Tipo).ToColumn("tipo");
        }
    }
}