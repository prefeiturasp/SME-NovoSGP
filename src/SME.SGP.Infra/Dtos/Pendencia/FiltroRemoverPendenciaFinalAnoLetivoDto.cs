using System;

namespace SME.SGP.Infra
{
    public class FiltroRemoverPendenciaFinalAnoLetivoDto : IEquatable<FiltroRemoverPendenciaFinalAnoLetivoDto>
    {
        public FiltroRemoverPendenciaFinalAnoLetivoDto()
        {
        }

        public FiltroRemoverPendenciaFinalAnoLetivoDto(int anoLetivo, long dreId)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
        }

        public FiltroRemoverPendenciaFinalAnoLetivoDto(int anoLetivo, long dreId, string codigoUe) :
            this(anoLetivo, dreId)
        {
            CodigoUe = codigoUe;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public string CodigoUe { get; set; }

        public bool Equals(FiltroRemoverPendenciaFinalAnoLetivoDto other)
        {
            return AnoLetivo == other.AnoLetivo &&
                   DreId == other.DreId &&
                   CodigoUe == other.CodigoUe;
        }
    }
}
