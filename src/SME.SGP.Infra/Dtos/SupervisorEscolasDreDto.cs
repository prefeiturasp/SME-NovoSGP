namespace SME.SGP.Infra
{
    public class SupervisorEscolasDreDto : AuditoriaDto
    {
        public string DreId { get; set; }
        public string EscolaId { get; set; }
        public string SupervisorId { get; set; }
        public bool Excluido { get; set; }
    }
}