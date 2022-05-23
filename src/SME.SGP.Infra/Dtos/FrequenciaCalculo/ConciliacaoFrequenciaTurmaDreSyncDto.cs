namespace SME.SGP.Infra.Dtos
{
    public class ConciliacaoFrequenciaTurmaDreSyncDto
    {
        public ConciliacaoFrequenciaTurmaDreSyncDto(long dreId, int anoLetivo)
        {
            DreId = dreId;
            AnoLetivo = anoLetivo;
        }

        public long DreId { get; set; }
        public int AnoLetivo { get; set; }
    }
}
