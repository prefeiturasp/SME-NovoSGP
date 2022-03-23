namespace SME.SGP.Infra
{
    public class ConselhoDeClasseComponenteSinteseDto
    {
        public long Codigo { get; set; }
        public string Nome { get; set; }
        public int? TotalFaltas { get; set; }
        public string PercentualFrequencia { get; set; }
        public int ParecerFinalId { get; set; }
        public string ParecerFinal { get; set; }
    }
}
