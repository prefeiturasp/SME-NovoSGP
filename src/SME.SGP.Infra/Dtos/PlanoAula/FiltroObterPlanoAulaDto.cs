namespace SME.SGP.Infra
{
    public class FiltroObterPlanoAulaDto
    {
        public FiltroObterPlanoAulaDto(long aulaId, long turmaId, long? componenteCurricularId)
        {
            AulaId = aulaId;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long AulaId { get; set; }
        public long TurmaId { get; set; }
        public long? ComponenteCurricularId { get; set; }
    }
}
