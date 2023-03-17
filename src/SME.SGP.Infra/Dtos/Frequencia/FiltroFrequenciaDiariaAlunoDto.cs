namespace SME.SGP.Infra
{
    public class FiltroFrequenciaDiariaAlunoDto
    {
        public FiltroFrequenciaDiariaAlunoDto(long turmaId, long componenteCurricularId, long alunoCodigo, int bimestre, int? semestre = 0)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            Semestre = semestre;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public int? Semestre { get; set; }
    }
}
