namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoGlobal : EntidadeBase
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int Modalidade { get; set; }
        public int TotalAulas { get; set; }
        public int TotalAusencias { get; set; }
        public int TotalCompensacoes { get; set; }
        public decimal PercentualFrequencia { get; set; }
        public int TotalAlunos { get; set; }
    }
}
