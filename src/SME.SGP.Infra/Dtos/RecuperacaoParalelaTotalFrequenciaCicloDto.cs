namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaTotalFrequenciaCicloDto
    {
        public string Chave { get; set; }
        public int CodigoCiclo { get; set; }
        public string Descricao { get; set; }
        public double Porcentagem { get; set; }
        public int Quantidade { get; set; }
        public double TotalPorcentagem { get; set; }
        public int TotalQuantidade { get; set; }
    }
}