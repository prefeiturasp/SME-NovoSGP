namespace SME.SGP.Infra
{
    public class DreUeDto
    {
        public DreUeDto(long dreId, long ueId = 0)
        {
            DreId = dreId;
            UeId = ueId;
        }

        public long DreId { get; set; }
        public long UeId { get; set; }
    }
}
