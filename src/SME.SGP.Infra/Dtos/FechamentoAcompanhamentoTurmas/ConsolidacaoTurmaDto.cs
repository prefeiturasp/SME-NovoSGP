namespace SME.SGP.Infra
{
    public class ConsolidacaoTurmaDto
    {
        public ConsolidacaoTurmaDto()
        {
        }

        public ConsolidacaoTurmaDto(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
    }
}
