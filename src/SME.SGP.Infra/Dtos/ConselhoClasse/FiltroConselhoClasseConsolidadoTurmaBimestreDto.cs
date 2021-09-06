namespace SME.SGP.Infra
{
    public class FiltroConselhoClasseConsolidadoTurmaBimestreDto
    {
        public FiltroConselhoClasseConsolidadoTurmaBimestreDto(long turmaId, int bimestre, int situacaoConselhoClasse)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
            SituacaoConselhoClasse = situacaoConselhoClasse;
        }

        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public int SituacaoConselhoClasse { get; set; }
    }
}
