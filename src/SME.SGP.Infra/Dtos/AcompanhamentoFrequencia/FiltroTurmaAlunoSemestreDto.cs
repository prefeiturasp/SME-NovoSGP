namespace SME.SGP.Infra
{
    public class FiltroTurmaAlunoSemestreDto

    {
        public FiltroTurmaAlunoSemestreDto(long turmaId, long alunoCodigo, int semestre)
        {
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
            Semestre = semestre;
        }

        public long TurmaId { get; set; }
        public long AlunoCodigo { get; set; }
        public int Semestre { get; set; }
    }
}