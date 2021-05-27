namespace SME.SGP.Infra
{
    public class MensagemConsolidacaoConselhoClasseAlunoDto
    {
        public MensagemConsolidacaoConselhoClasseAlunoDto(string alunoCodigo, long turmaId, int bimestre)
        {
            AlunoCodigo = alunoCodigo;
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public string AlunoCodigo { get; set; }
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
    }
}
