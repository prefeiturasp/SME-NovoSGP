namespace SME.SGP.Infra
{
    public class FiltroAcompanhamentoTurmaAlunoSemestreDto
    {
        public long TurmaId { get; set; }
        public string AlunoId { get; set; }
        public int Semestre { get; set; }

        public FiltroAcompanhamentoTurmaAlunoSemestreDto(long turmaId, string alunoId, int semestre)
        {
            TurmaId = turmaId;
            AlunoId = alunoId;
            Semestre = semestre;
        }
    }
}
