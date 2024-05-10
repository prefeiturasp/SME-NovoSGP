namespace SME.SGP.Infra
{
    public class PendenciaFechamentoDetalhamentoDto
    {
        public long PendenciaId { get; set; }
        public long FechamentoId { get; set; }
        public string Descricao { get; set; }
        public string Detalhamento { get; set; }
        public string DescricaoHtml { get; set; }
    }
}
