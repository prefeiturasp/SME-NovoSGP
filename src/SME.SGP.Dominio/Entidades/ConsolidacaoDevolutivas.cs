namespace SME.SGP.Dominio
{
    public class ConsolidacaoDevolutivas
    {
        public ConsolidacaoDevolutivas() { }

        public long Id { get; set; }
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }

        public int QuantidadeEstimadaDevolutivas { get; set; }
        public int QuantidadeRegistradaDevolutivas { get; set; }

        public int QuantidadeComDevolutiva { get; set; }
        public int QuantidadeComDevolutivaPendente { get; set; }

        public int QuantidadeDiariosDeBordoComReflexoesEPlanejamentosPrenchidos { get; set; }
        public int QuantidadeDiariosDeBordoSemReflexoesEPlanejamentosPrenchidos { get; set; }
    }
}