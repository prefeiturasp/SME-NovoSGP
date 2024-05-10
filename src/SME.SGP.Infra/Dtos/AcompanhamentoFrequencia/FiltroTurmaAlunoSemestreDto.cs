namespace SME.SGP.Infra
{
    public class FiltroTurmaAlunoSemestreDto

    {
        public FiltroTurmaAlunoSemestreDto(long turmaId, long alunoCodigo, int semestre, long componenteCurricularId = 0)
        {
            TurmaId = turmaId;

            AlunoCodigo = alunoCodigo;

            Semestre = semestre;

            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }

        public long AlunoCodigo { get; set; }

        public int Semestre { get; set; }

        public long ComponenteCurricularId { get; set; }        
    }
}