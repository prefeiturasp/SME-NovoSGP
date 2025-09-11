namespace SME.SGP.Dominio
{
    public class ConsolidacaoMatriculaTurma
    {
        public ConsolidacaoMatriculaTurma(long turmaId, int quantidade)
        {
            TurmaId = turmaId;
            Quantidade = quantidade;
        }

        public long Id { get; set; }
        public long TurmaId { get; set; }
        public int Quantidade { get; set; }
    }
}
