namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class FiltroPainelEducacionalAnoLetivoPeriodo : FiltroPainelEducacionalDreUe
    {
        public int AnoLetivo { get; set; } 
        public int Periodo { get; set; }
        public int SerieAno { get; set; }
    }
}
