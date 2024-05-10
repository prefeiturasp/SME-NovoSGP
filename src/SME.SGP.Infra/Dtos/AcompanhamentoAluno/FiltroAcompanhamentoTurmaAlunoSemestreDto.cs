namespace SME.SGP.Infra
{
    public class FiltroAcompanhamentoTurmaAlunoSemestreDto
    {
        public long TurmaId { get; set; }
        public string AlunoId { get; set; }
        public int Semestre { get; set; }
        public long ComponenteCurricularId { get; }

        public FiltroAcompanhamentoTurmaAlunoSemestreDto(long turmaId, string alunoId, int semestre, long componenteCurricularId)
        {
            TurmaId = turmaId;
            AlunoId = alunoId;
            Semestre = semestre;
            ComponenteCurricularId = componenteCurricularId;
        }
    }
}
