namespace SME.SGP.Dominio.Entidades
{
   public class PainelEducacionalRegistroFrequenciaAgrupamentoEscola : EntidadeBase
    {
        public int TotalAulas { get; set; }
        public int TotalAusencias { get; set; }
        public int TotalCompensacoes { get; set; }
        public decimal PercentualFrequencia { get; set; }
        public int TotalAlunos { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string UE { get; set; }
        public string DRE { get; set; }
        public int Mes { get; set; }
    }
}
