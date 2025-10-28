namespace SME.SGP.Dominio.Entidades
{
    public class ConsolidacaoAlfabetizacaoCriticaEscrita
    {
        public long Id { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string DreNome { get; set; }
        public string UeNome { get; set; }
        public int Posicao { get; set; }
        public long TotalAlunosNaoAlfabetizados { get; set; }
        public decimal PercentualTotalAlunos { get; set; }
        public int AnoLetivo { get; set; }
    }
}
