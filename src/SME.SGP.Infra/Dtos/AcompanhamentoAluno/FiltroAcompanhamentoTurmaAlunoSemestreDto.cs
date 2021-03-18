namespace SME.SGP.Infra
{
    public class FiltroAcompanhamentoTurmaAlunoSemestreDto
    {
        public string TurmaId { get; set; }
        public string AlunoId { get; set; }
        public int Semestre { get; set; }

        public FiltroAcompanhamentoTurmaAlunoSemestreDto(string turmaId, string alunoId, int semestre)
        {
            TurmaId = turmaId;
            AlunoId = alunoId;
            Semestre = semestre;
        }
    }
}
