namespace SME.SGP.Infra
{
    public class FiltroAcompanhamentoAprendizagemAlunoTurmaDTO
    {
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public int Semestre { get; set; }
        public int QuantidadeAlunosTurma { get; set; }

        public FiltroAcompanhamentoAprendizagemAlunoTurmaDTO(long turmaId, int anoLetivo, int semestre, int quantidadeAlunosTurma)
        {
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
            Semestre = semestre;
            QuantidadeAlunosTurma = quantidadeAlunosTurma;
        }
    }
}
