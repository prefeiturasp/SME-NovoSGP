namespace SME.SGP.Infra
{
    public class FiltroRegistroIndividualBase
    {
        public FiltroRegistroIndividualBase(long turmaId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }

        public long ComponenteCurricularId { get; set; }
    }
}
