namespace SME.SGP.Dominio
{
    public class ConsolidacaoDevolutivas
    {
        public ConsolidacaoDevolutivas()
        {

        }
        public ConsolidacaoDevolutivas(long turmaId, int quantidadeEstimadaDevolutivas, int quantidadeRegistradaDevolutivas)
        {
            TurmaId = turmaId;
            QuantidadeEstimadaDevolutivas = quantidadeEstimadaDevolutivas;
            QuantidadeRegistradaDevolutivas = quantidadeRegistradaDevolutivas;
        }

        public long Id { get; set; }
        public long TurmaId { get; set; }

        public int QuantidadeEstimadaDevolutivas { get; set; }
        public int QuantidadeRegistradaDevolutivas { get; set; }
    }
}