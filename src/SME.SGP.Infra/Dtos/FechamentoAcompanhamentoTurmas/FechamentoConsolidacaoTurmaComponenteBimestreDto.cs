namespace SME.SGP.Infra
{
    public class FechamentoConsolidacaoTurmaComponenteBimestreDto
    {
        public FechamentoConsolidacaoTurmaComponenteBimestreDto(long turmaId, int bimestre, long componenteCurricularId)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; }
        public int Bimestre { get; }
        public long ComponenteCurricularId { get; }
    }
}

