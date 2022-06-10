namespace SME.SGP.Infra
{
    public class FiltroFrequenciaDiariaAlunoDto
    {
        public FiltroFrequenciaDiariaAlunoDto(long turmaId, long componenteCurricularId, long alunoCodigo, int bimestre)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
    }
}
