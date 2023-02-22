namespace SME.SGP.Infra
{
    public class FiltroExcluirPendenciaCalendarioAnoAnteriorPorUeDto
    {
        public FiltroExcluirPendenciaCalendarioAnoAnteriorPorUeDto(int? anoLetivo, long ueId)
        {
            AnoLetivo = anoLetivo;
            UeId = ueId;
        }

        public int? AnoLetivo { get; set; }
        public long UeId { get; set; }
    }
}
