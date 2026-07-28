using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class WorkflowAprovacaoNivelUsuario
    {
        [Key]
        public long Id { get; set; }
        [Computed]
        public Usuario Usuario { get; set; }
        public long UsuarioId { get; set; }
        [Computed]
        public WorkflowAprovacaoNivel WorkflowAprovacaoNivel { get; set; }
        public long WorkflowAprovacaoNivelId { get; set; }
    }
}