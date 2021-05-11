namespace SME.SGP.Dominio
{
    public class ConsolidacaoDevolutivas
    {
        public ConsolidacaoDevolutivas() { }

        public long Id { get; set; }
        public long TurmaId { get; set; }

        public int QuantidadeEstimadaDevolutivas { get; set; }
        public int QuantidadeRegistradaDevolutivas { get; set; }
    }
}