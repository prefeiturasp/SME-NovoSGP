namespace SME.SGP.Dominio
{
    public class WorkflowAprovacaoNivel : EntidadeBase
    {
        public string Descricao { get; set; }
        public int Nivel { get; set; }
        public string UsuarioId { get; set; }
        public long WorkflowId { get; set; }
        public WorkflowAprovacao Workflow { get; set; }
    }
}