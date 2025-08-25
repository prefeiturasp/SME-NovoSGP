namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto
    {
        public string Modalidade { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int TotalAulas { get; set; }
        public int TotalAusencias { get; set; }
        public decimal PercentualFrequencia { get; set; }
    }
}
