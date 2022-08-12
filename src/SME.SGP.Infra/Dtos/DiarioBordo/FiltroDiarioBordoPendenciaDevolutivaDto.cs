namespace SME.SGP.Infra
{
    public class FiltroDiarioBordoPendenciaDevolutivaDto
    {
        public FiltroDiarioBordoPendenciaDevolutivaDto(int anoLetivo,long dreId=0, string ueCodigo = "", long turmaId = 0, long ueId = 0)
        {
            DreId = dreId;
            UeCodigo = ueCodigo;
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
            UeId = ueId;
        }
        public long DreId { get; set; }
        public int AnoLetivo { get; set; }
        public string UeCodigo { get; set; }
        public long UeId { get; set; }
        public long TurmaId { get; set; }
    }
}
