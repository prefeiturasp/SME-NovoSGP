namespace SME.SGP.Infra
{
    public class FiltroConselhoClasseConsolidadoTurmaBimestreDto
    {
        public FiltroConselhoClasseConsolidadoTurmaBimestreDto(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }

        public int Bimestre { get; set; }
    }
}
