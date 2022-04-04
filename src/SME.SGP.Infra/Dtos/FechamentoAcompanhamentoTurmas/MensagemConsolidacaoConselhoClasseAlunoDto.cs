namespace SME.SGP.Infra
{
    public class MensagemConsolidacaoConselhoClasseAlunoDto
    {
        public MensagemConsolidacaoConselhoClasseAlunoDto() { }

        public MensagemConsolidacaoConselhoClasseAlunoDto(string alunoCodigo, long turmaId, int bimestre, bool inativo, double? nota = null, long? conceitoId = null, long? componenteCurricularId = null)
        {
            AlunoCodigo = alunoCodigo;
            TurmaId = turmaId;
            Bimestre = bimestre;
            Inativo = inativo;
            Nota = nota;
            ConceitoId = conceitoId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public string AlunoCodigo { get; set; }
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public bool Inativo { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public long? ComponenteCurricularId { get; set; }
    }
}
