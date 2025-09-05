namespace SME.SGP.Infra.Dtos.Sondagem
{
    public class SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto
    {
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int QuantidadeNaoAlfabetizados { get; set; }
        public decimal PercentualNaoAlfabetizados { get; set; }
    }
}
