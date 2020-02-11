namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaTotalFrequenciaAnoDto
    {
        public string Chave { get; set; }
        public int CodigoAno { get; set; }
        public string Descricao { get; set; }
        public double Porcentagem { get; set; }
        public int Quantidade { get; set; }
        public double TotalPorcentagem { get; set; }
        public int TotalQuantidade { get; set; }
    }
}