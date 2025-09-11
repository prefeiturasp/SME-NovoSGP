namespace SME.SGP.Dominio
{
    public class WorkflowAprovacaoNivelUsuario
    {
        public long Id { get; set; }
        public Usuario Usuario { get; set; }
        public long UsuarioId { get; set; }
        public WorkflowAprovacaoNivel WorkflowAprovacaoNivel { get; set; }
        public long WorkflowAprovacaoNivelId { get; set; }
    }
}