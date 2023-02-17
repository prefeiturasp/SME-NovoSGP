namespace SME.SGP.Infra
{
    public class FiltroRemoverPendenciaFinalAnoLetivoDto
    {
        public FiltroRemoverPendenciaFinalAnoLetivoDto(int anoLetivo, long dreId)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
        }

        public FiltroRemoverPendenciaFinalAnoLetivoDto(int anoLetivo, long dreId, long ueId) : 
            this(anoLetivo, dreId)
        {
            UeId = ueId;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
    }
}
