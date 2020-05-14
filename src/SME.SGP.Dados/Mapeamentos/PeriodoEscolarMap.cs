using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoEscolarMap : BaseMap<PeriodoEscolar>
    {
        public PeriodoEscolarMap()
        {
            ToTable("periodo_escolar");
            Map(c => c.TipoCalendarioId).ToColumn("tipo_calendario_id");
            Map(c => c.TipoCalendario).Ignore();
            Map(c => c.PeriodoFim).ToColumn("periodo_fim");
            Map(c => c.PeriodoInicio).ToColumn("periodo_inicio");
        }
    }
}