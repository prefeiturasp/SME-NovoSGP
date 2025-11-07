namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalAprovacaoDto
    {
        public string Modalidade { get; set; }
        public int TotalAulas { get; set; }
        public int TotalAusencias { get; set; }
        public int TotalCompensacoes { get; set; }
        public decimal PercentualFrequencia { get; set; }
        public int TotalAlunos { get; set; }
    }
}
