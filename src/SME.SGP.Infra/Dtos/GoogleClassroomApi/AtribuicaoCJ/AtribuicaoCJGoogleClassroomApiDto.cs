namespace SME.SGP.Infra
{
    public class AtribuicaoCJGoogleClassroomApiDto
    {
        public long Rf { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }

        public AtribuicaoCJGoogleClassroomApiDto(long rf, long turmaId, long componenteCurricularId)
        {
            Rf = rf;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }
    }
}