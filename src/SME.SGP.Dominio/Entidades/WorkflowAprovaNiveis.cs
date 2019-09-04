namespace SME.SGP.Dominio
{
    public class WorkflowAprovaNiveis : EntidadeBase
    {
        public string Descricao { get; set; }
        public WorkflowAprovaNiveisStatus Status { get; set; }
        public string UsuarioId { get; set; }
    }
}