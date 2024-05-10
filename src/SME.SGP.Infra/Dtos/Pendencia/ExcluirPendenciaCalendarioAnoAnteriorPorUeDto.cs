using System;
using System.Linq;

namespace SME.SGP.Infra
{
    public class ExcluirPendenciaCalendarioAnoAnteriorPorUeDto : IEquatable<ExcluirPendenciaCalendarioAnoAnteriorPorUeDto>
    {
        public ExcluirPendenciaCalendarioAnoAnteriorPorUeDto(int? anoLetivo, long ueId, long[] pendenciasId)
        {
            AnoLetivo = anoLetivo;
            UeId = ueId;
            PendenciasId = pendenciasId;
        }

        public int? AnoLetivo { get; set; }
        public long UeId { get; set; }
        public long[] PendenciasId { get; set; }

        public bool Equals(ExcluirPendenciaCalendarioAnoAnteriorPorUeDto other)
        {
            return AnoLetivo == other.AnoLetivo &&
                   UeId == other.UeId &&
                   PendenciasId.SequenceEqual(other.PendenciasId);
        }
    }
}
