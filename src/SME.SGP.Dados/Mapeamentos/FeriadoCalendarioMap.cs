using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FeriadoCalendarioMap : BaseEntityMap<FeriadoCalendario>
    {
        public FeriadoCalendarioMap()
        {
            ToTable("feriado_calendario");
            Map(nameof(FeriadoCalendario.Abrangencia), "abrangencia");
            Map(nameof(FeriadoCalendario.Ativo), "ativo");
            Map(nameof(FeriadoCalendario.DataFeriado), "data_feriado");
            Map(nameof(FeriadoCalendario.Excluido), "excluido");
            Map(nameof(FeriadoCalendario.Nome), "nome");
            Map(nameof(FeriadoCalendario.Tipo), "tipo");
        }
    }
}