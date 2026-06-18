namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoRegistrosPedagogicosPorUeDto
    {
        public FiltroConsolidacaoRegistrosPedagogicosPorUeDto(long ueId, int anoLetivo)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
    }
}
