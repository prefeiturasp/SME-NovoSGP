namespace SME.SGP.Dominio
{
    public class FechamentoFinal : EntidadeBase
    {
        public string AlunoCodigo { get; set; }
        public Conceito Conceito { get; set; }
        public long? ConceitoId { get; set; }
        public long DisciplinaCodigo { get; set; }
        public bool EhRegencia { get; set; }
        public double? Nota { get; set; }
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }

        public void AtualizaConceito(Conceito conceito)
        {
            if (conceito != null)
            {
                Conceito = conceito;
                ConceitoId = conceito.Id;
            }
        }

        public void AtualizarTurma(Turma turma)
        {
            if (turma != null)
            {
                Turma = turma;
                TurmaId = turma.Id;
            }
        }
    }
}