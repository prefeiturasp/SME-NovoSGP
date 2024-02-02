using System;

namespace SME.SGP.Infra
{
    public class ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto : IEquatable<ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto>
    {
        public ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto(int? anoLetivo, long ueId)
        {
            AnoLetivo = anoLetivo;
            UeId = ueId;
        }

        public int? AnoLetivo { get; set; }
        public long UeId { get; set; }

        public bool Equals(ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto other)
        {
            return AnoLetivo == other.AnoLetivo &&
                UeId == other.UeId;
        }
    }
}
