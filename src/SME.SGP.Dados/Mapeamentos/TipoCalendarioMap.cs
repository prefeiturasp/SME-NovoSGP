using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoCalendarioMap : BaseMap<TipoCalendario>
    {
        public TipoCalendarioMap()
        {
            ToTable("tipo_calendario");

            Map(nameof(TipoCalendario.AnoLetivo), "ano_letivo");
            Map(nameof(TipoCalendario.Excluido), "excluido");
            Map(nameof(TipoCalendario.Migrado), "migrado");
            Map(nameof(TipoCalendario.Modalidade), "modalidade");
            Map(nameof(TipoCalendario.Nome), "nome");
            Map(nameof(TipoCalendario.Periodo), "periodo");
            Map(nameof(TipoCalendario.Situacao), "situacao");
            Map(nameof(TipoCalendario.Semestre), "semestre");
        }
    }
}