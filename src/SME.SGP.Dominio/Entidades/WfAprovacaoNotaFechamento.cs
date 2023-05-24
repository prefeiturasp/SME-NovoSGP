namespace SME.SGP.Dominio
{
    public class WfAprovacaoNotaFechamento : EntidadeBase
    {
        public WfAprovacaoNotaFechamento() { }
        public long? WfAprovacaoId { get; set; }
        public WorkflowAprovacao WfAprovacao { get; set; }
        public long FechamentoNotaId { get; set; }
        public FechamentoNota FechamentoNota { get; set; }

        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public Conceito Conceito { get; set; }
        public bool Excluido { get; set; }
        public double? NotaAnterior { get; set; }
        public long? ConceitoIdAnterior { get; set; }
    }
}
