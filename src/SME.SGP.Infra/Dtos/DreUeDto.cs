namespace SME.SGP.Infra
{
    public class DreUeDto
    {
        public DreUeDto(long dreId, long ueId = 0, string codigoUe = "")
        {
            DreId = dreId;
            UeId = ueId;
            CodigoUe = codigoUe;
        }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public string CodigoUe { get; set; }
    }
}
