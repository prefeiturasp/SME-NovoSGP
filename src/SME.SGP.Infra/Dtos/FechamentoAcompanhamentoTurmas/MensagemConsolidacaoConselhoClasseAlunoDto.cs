namespace SME.SGP.Infra
{
    public class MensagemConsolidacaoConselhoClasseAlunoDto
    {
        public MensagemConsolidacaoConselhoClasseAlunoDto(string alunoCodigo, long turmaId, int bimestre, bool inativo)
        {
            AlunoCodigo = alunoCodigo;
            TurmaId = turmaId;
            Bimestre = bimestre;
            Inativo = inativo;
        }

        public string AlunoCodigo { get; set; }
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public bool Inativo { get; set; }
    }
}
