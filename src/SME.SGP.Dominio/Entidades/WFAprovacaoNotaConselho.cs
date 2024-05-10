namespace SME.SGP.Dominio
{
    public class WFAprovacaoNotaConselho : EntidadeBase
    {
        public long? WfAprovacaoId { get; set; }
        public WorkflowAprovacao WfAprovacao { get; set; }
        public long? ConselhoClasseNotaId { get; set; }
        public ConselhoClasseNota ConselhoClasseNota { get; set; }
        public long UsuarioSolicitanteId { get; set; }

        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public Conceito Conceito { get; set; }
        public double? NotaAnterior { get; set; }
        public long? ConceitoIdAnterior { get; set; }
        public bool Excluido { get; set; }
    }
}
