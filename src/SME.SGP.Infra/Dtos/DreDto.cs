namespace SME.SGP.Infra
{
    public class DreDto
    {
        public DreDto(long dreId)
        {
            DreCodigo = dreId;
        }

        public long DreCodigo { get; set; }
    }
}
