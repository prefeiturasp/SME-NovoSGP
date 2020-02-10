namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaResumoFrequenciaDto
    {
        public RecuperacaoParalelaTotalAnoDto Anos { get; set; }
        public RecuperacaoParalelaTotalCicloDto Ciclos { get; set; }
        public string FrequenciaDescricao { get; set; }
        public double PorcentagemTotalFrequencia { get; set; }
        public int QuantidadeTotalFrequencia { get; set; }
        public RecuperacaoParalelaResumoTotalLihas TotalLinhas { get; set; }
    }
}