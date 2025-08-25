namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoMensal : EntidadeBase
    {
        public int Modalidade { get; set; }
        public long AnoLetivo { get; set; }
        public int Mes { get; set; }
        public int TotalAulas { get; set; }
        public int TotalFaltas { get; set; }
        public decimal PercentualFrequencia { get; set; }
    }
}
