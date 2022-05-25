namespace SME.SGP.Infra.Dtos
{
    public class ConciliacaoFrequenciaAnoSyncDto
    {
        public ConciliacaoFrequenciaAnoSyncDto(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }
}
