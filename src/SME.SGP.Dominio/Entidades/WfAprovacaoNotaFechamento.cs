using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class WfAprovacaoNotaFechamento : EntidadeBase
    {
        public WfAprovacaoNotaFechamento() { }
        public long? WfAprovacaoId { get; set; }
        [Computed]
        public WorkflowAprovacao WfAprovacao { get; set; }
        public long FechamentoNotaId { get; set; }
        [Computed]
        public FechamentoNota FechamentoNota { get; set; }

        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        [Computed]
        public Conceito Conceito { get; set; }
        public bool Excluido { get; set; }
        public double? NotaAnterior { get; set; }
        public long? ConceitoIdAnterior { get; set; }
    }
}
