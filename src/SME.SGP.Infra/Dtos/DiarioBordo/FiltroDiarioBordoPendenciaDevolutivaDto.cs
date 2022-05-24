namespace SME.SGP.Infra
{
    public class FiltroDiarioBordoPendenciaDevolutivaDto
    {
        public FiltroDiarioBordoPendenciaDevolutivaDto(int anoLetivo,long dreCodigo, string ueCodigo = "", string turmaCodigo = "", string componenteCodigo = "")
        {
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            TurmaCodigo = turmaCodigo;
            ComponenteCodigo = componenteCodigo;
            AnoLetivo = anoLetivo;
        }

        public long DreCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public string UeCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public string ComponenteCodigo { get; set; }
    }
}
