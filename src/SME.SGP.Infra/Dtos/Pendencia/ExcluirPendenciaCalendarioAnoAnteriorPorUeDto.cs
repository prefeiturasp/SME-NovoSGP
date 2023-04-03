using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ExcluirPendenciaCalendarioAnoAnteriorPorUeDto
    {
        public ExcluirPendenciaCalendarioAnoAnteriorPorUeDto(int? anoLetivo, long ueId, long[] pendenciaId)
        {
            AnoLetivo = anoLetivo;
            UeId = ueId;
            PendenciaId = pendenciaId;

        }

        public int? AnoLetivo { get; set; }
        public long UeId { get; set; }
        public long[] PendenciaId { get; set; }
    }
}
