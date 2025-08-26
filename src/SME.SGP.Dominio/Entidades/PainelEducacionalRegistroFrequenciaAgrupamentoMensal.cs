namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoMensal : EntidadeBase
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int Modalidade { get; set; }
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public int TotalAulas { get; set; }
        public int TotalFaltas { get; set; }
        public decimal PercentualFrequencia { get; set; }
    }
}
