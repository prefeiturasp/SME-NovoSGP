namespace SME.SGP.Infra
{
    public class MensagemConsolidarTurmaConselhoClasseAlunoPorAlunoDto
    {
        public MensagemConsolidarTurmaConselhoClasseAlunoPorAlunoDto(long consolidacaoId, long turmaId, string alunoCodigo)
        {
            this.ConsolidacaoId = consolidacaoId;
            this.TurmaId = turmaId;
            this.AlunoCodigo = alunoCodigo;
        }

        public long ConsolidacaoId { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
    }
}
