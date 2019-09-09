namespace SME.SGP.Dominio
{
    public class WorkflowAprovacaoNivel : EntidadeBase
    {
        public WorkflowAprovacaoNivel()
        {
            Status = WorkflowAprovacaoNivelStatus.SemStatus;
        }

        public string Descricao { get; set; }
        public int Nivel { get; set; }
        public WorkflowAprovacaoNivelStatus Status { get; set; }
        public string UsuarioId { get; set; }
        public WorkflowAprovacao Workflow { get; set; }
        public long WorkflowId { get; set; }
    }
}