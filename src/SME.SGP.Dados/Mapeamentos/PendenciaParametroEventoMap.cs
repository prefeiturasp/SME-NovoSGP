using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class PendenciaParametroEventoMap : BaseMap<PendenciaParametroEvento>
    {
        public PendenciaParametroEventoMap()
        {
            ToTable("pendencia_parametro_evento");
            Map(nameof(PendenciaParametroEvento.PendenciaCalendarioUeId), "pendencia_calendario_ue_id");
            Map(nameof(PendenciaParametroEvento.ParametroSistemaId), "parametro_sistema_id");
            Map(nameof(PendenciaParametroEvento.QuantidadeEventos), "quantidade_eventos");
        }
    }
}