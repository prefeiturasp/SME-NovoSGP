using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PendenciaCalendarioUeMap : BaseEntityMap<PendenciaCalendarioUe>
    {
        public PendenciaCalendarioUeMap()
        {
            ToTable("pendencia_calendario_ue");
            Map(nameof(PendenciaCalendarioUe.PendenciaId), "pendencia_id");
            Map(nameof(PendenciaCalendarioUe.UeId), "ue_id");
            Map(nameof(PendenciaCalendarioUe.TipoCalendarioId), "tipo_calendario_id");
        }
    }
}