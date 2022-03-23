namespace SME.SGP.Infra
{
    public class FiltroDreUeTurmaDto
    {
        public FiltroDreUeTurmaDto(int anoLetivo, long dreId, string ueCodigo = "", string turmaCodigo = "")
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeCodigo = ueCodigo;
            TurmaCodigo = turmaCodigo;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public string UeCodigo { get; set; }
        public string TurmaCodigo { get; set; }
    }
}
