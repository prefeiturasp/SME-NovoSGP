namespace SME.SGP.Dominio
{
    public class WorkflowAprovaNivel : EntidadeBase
    {
        public WorkflowAprovaNivel()
        {
            Status = WorkflowAprovaNiveisStatus.SemStatus;
        }

        public int Ano { get; set; }
        public string Chave { get; set; }
        public string Descricao { get; set; }
        public string DreId { get; set; }
        public string EscolaId { get; set; }
        public int Nivel { get; set; }
        public WorkflowAprovaNiveisStatus Status { get; set; }
        public string TurmaId { get; set; }
        public string UsuarioId { get; set; }
    }
}