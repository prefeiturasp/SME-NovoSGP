namespace SME.SGP.Dominio
{
    public class WorkflowAprovaNiveis : EntidadeBase
    {
        public string Descricao { get; set; }
        public WorkflowAprovaNiveisStatus Status { get; set; }
        public string UsuarioId { get; set; }
        public string TurmaId { get; set; }
        public string EscolaId { get; set; }
        public int Ano { get; set; }
        public string DreId { get; set; }
    }
}