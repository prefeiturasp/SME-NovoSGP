using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoEscolarMap : BaseMap<PeriodoEscolar>
    {
        public PeriodoEscolarMap()
        {
            ToTable("periodo_escolar");
            Map(nameof(PeriodoEscolar.Bimestre), "bimestre");
            Map(nameof(PeriodoEscolar.Migrado), "migrado");
            Map(nameof(PeriodoEscolar.PeriodoFim), "periodo_fim");
            Map(nameof(PeriodoEscolar.PeriodoInicio), "periodo_inicio");
            Map(nameof(PeriodoEscolar.TipoCalendarioId), "tipo_calendario_id");
        }
    }
}