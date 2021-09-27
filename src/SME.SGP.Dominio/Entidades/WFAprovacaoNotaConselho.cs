namespace SME.SGP.Dominio.Entidades
{
    public class WFAprovacaoNotaConselho
    {
        public long Id { get; set; }
        public long WfAprovacaoId { get; set; }
        public WorkflowAprovacao WfAprovacao { get; set; }
        public long ConselhoClasseNotaId { get; set; }
        public ConselhoClasseNota ConselhoClasseNota { get; set; }

        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public Conceito Conceito { get; set; }
    }
}
