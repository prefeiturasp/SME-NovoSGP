namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class FiltroPainelEducacionalAnoLetivoBimestre : FiltroPainelEducacionalDreUe
    {
        public int AnoLetivo { get; set; } 
        public int Bimestre { get; set; }
        public int SerieAno { get; set; }
    }
}
